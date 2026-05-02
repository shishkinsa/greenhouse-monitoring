using GM.WebApi.Infrastructure.Interfaces.DataAccess;
using GM.WebApi.UseCases.Handlers.Region.DTOs;
using GM.WebApi.UseCases.Handlers.Region.Queries.ListRegions.Responses;

using Microsoft.EntityFrameworkCore;

using Requestum.Contract;

namespace GM.WebApi.UseCases.Handlers.Region.Queries.ListRegions;

public class ListRegionsQueryHandler : IAsyncQueryHandler<ListRegionsQuery, RegionListResponse>
{
    private readonly IDbContext _db;

    public ListRegionsQueryHandler(IDbContext db) => _db = db;

    public async Task<RegionListResponse> HandleAsync(ListRegionsQuery query, CancellationToken cancellationToken = default)
    {
        var page = Math.Max(1, query.Page);
        var pageSize = query.PageSize < 1 ? 25 : Math.Clamp(query.PageSize, 1, 100);

        var ordered = _db.Regions.AsNoTracking().OrderBy(r => r.Code);

        var totalItems = await ordered.CountAsync(cancellationToken);

        var items = await ordered
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(r => new RegionDto(
                r.Id,
                r.Code,
                r.Name,
                r.IsActive,
                r.CreatedAt,
                r.UpdatedAt))
            .ToListAsync(cancellationToken);

        return new RegionListResponse
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalItems = totalItems
        };
    }
}
