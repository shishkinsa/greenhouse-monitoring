using GM.WebApi.Infrastructure.Interfaces.DataAccess;
using GM.WebApi.UseCases.Handlers.SensorTypes.Mappings;
using GM.WebApi.UseCases.Handlers.SensorTypes.Queries.ListSensorTypes.Responses;

using Microsoft.EntityFrameworkCore;

using Requestum.Contract;

namespace GM.WebApi.UseCases.Handlers.SensorTypes.Queries.ListSensorTypes;

public class ListSensorTypesQueryHandler : IAsyncQueryHandler<ListSensorTypesQuery, SensorTypeListResponse>
{
    private readonly IDbContext _db;

    public ListSensorTypesQueryHandler(IDbContext db) => _db = db;

    public async Task<SensorTypeListResponse> HandleAsync(
        ListSensorTypesQuery query,
        CancellationToken cancellationToken = default)
    {
        var page = Math.Max(1, query.Page);
        var pageSize = query.PageSize < 1 ? 25 : Math.Clamp(query.PageSize, 1, 100);

        var ordered = _db.SensorTypes.AsNoTracking().OrderBy(st => st.Code);

        var totalItems = await ordered.CountAsync(cancellationToken);

        var entities = await ordered
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var items = entities.Select(SensorTypeMappings.ToDto).ToList();

        return new SensorTypeListResponse
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalItems = totalItems,
        };
    }
}
