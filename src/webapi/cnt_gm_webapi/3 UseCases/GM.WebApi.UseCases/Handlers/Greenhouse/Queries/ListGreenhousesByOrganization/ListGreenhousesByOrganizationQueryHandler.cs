using FluentValidation;

using GM.WebApi.Infrastructure.Interfaces.DataAccess;
using GM.WebApi.UseCases.Exceptions;
using GM.WebApi.UseCases.Handlers.Greenhouse.DTOs;
using GM.WebApi.UseCases.Handlers.Greenhouse.Queries.ListGreenhousesByOrganization.Responses;

using Microsoft.EntityFrameworkCore;

using Requestum.Contract;

namespace GM.WebApi.UseCases.Handlers.Greenhouse.Queries.ListGreenhousesByOrganization;

public sealed class ListGreenhousesByOrganizationQueryHandler
    : IAsyncQueryHandler<ListGreenhousesByOrganizationQuery, GreenhouseListResponse>
{
    private readonly IDbContext _db;
    private readonly IValidator<ListGreenhousesByOrganizationQuery> _validator;

    public ListGreenhousesByOrganizationQueryHandler(
        IDbContext db,
        IValidator<ListGreenhousesByOrganizationQuery> validator)
    {
        _db = db;
        _validator = validator;
    }

    public async Task<GreenhouseListResponse> HandleAsync(
        ListGreenhousesByOrganizationQuery query,
        CancellationToken cancellationToken = default)
    {
        await _validator.ValidateAndThrowAsync(query, cancellationToken);

        var orgExists = await _db.Organizations.AnyAsync(
            o => o.Id == query.OrganizationId,
            cancellationToken);
        if (!orgExists)
        {
            throw new UseCaseNotFoundException(
                $"Организация с идентификатором {query.OrganizationId} не найдена.");
        }

        var page = Math.Max(1, query.Page);
        var pageSize = query.PageSize < 1 ? 25 : Math.Clamp(query.PageSize, 1, 100);

        var baseQuery =
            from g in _db.Greenhouses.AsNoTracking()
            join l in _db.Locations.AsNoTracking() on g.LocationId equals l.Id
            where g.OrganizationId == query.OrganizationId
            orderby g.Code
            select new { g, l };

        var totalItems = await baseQuery.CountAsync(cancellationToken);

        var items = await baseQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new GreenhouseDto(
                x.g.Id,
                x.g.Code,
                x.g.Name,
                x.g.OrganizationId,
                x.g.LocationId,
                x.g.AreaM2,
                x.g.Timezone,
                x.g.IsActive,
                x.g.CommissionedAt,
                x.g.CreatedAt,
                x.g.UpdatedAt,
                x.l.Address))
            .ToListAsync(cancellationToken);

        return new GreenhouseListResponse
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalItems = totalItems
        };
    }
}
