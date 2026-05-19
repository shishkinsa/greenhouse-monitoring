using GM.WebApi.Infrastructure.Interfaces.DataAccess;
using GM.WebApi.UseCases.Handlers.Location.DTOs;
using GM.WebApi.UseCases.Handlers.Location.Queries.ListLocations.Responses;

using Microsoft.EntityFrameworkCore;

using Requestum.Contract;

namespace GM.WebApi.UseCases.Handlers.Location.Queries.ListLocations;

public sealed class ListLocationsQueryHandler : IAsyncQueryHandler<ListLocationsQuery, LocationListResponse>
{
    private readonly IDbContext _db;

    public ListLocationsQueryHandler(IDbContext db) => _db = db;

    public async Task<LocationListResponse> HandleAsync(
        ListLocationsQuery query,
        CancellationToken cancellationToken = default)
    {
        var page = Math.Max(1, query.Page);
        var pageSize = query.PageSize < 1 ? 25 : Math.Clamp(query.PageSize, 1, 100);

        var ordered = _db.Locations.AsNoTracking().OrderBy(l => l.Code);

        var totalItems = await ordered.CountAsync(cancellationToken);

        var items = await ordered
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(l => new LocationDto(
                l.Id,
                l.RegionId,
                l.Code,
                l.Name,
                l.Address,
                l.Latitude,
                l.Longitude,
                l.IsActive,
                l.CreatedAt,
                l.UpdatedAt))
            .ToListAsync(cancellationToken);

        return new LocationListResponse
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalItems = totalItems,
        };
    }
}
