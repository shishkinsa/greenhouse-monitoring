using FluentValidation;

using GM.WebApi.Infrastructure.Interfaces.DataAccess;
using GM.WebApi.UseCases.Exceptions;
using GM.WebApi.UseCases.Handlers.Region.DTOs;
using GM.WebApi.UseCases.Handlers.Region.Queries.GetRegionById.Responses;

using Microsoft.EntityFrameworkCore;

using Requestum.Contract;

namespace GM.WebApi.UseCases.Handlers.Region.Queries.GetRegionById;

public class GetRegionByIdQueryHandler: IAsyncQueryHandler<GetRegionByIdQuery, GetRegionByIdResponse>
{
    private readonly IDbContext _db;
    private readonly IValidator<GetRegionByIdQuery> _validator;

    public GetRegionByIdQueryHandler(IDbContext db, IValidator<GetRegionByIdQuery> validator)
    {
        _db = db;
        _validator = validator;
    }

    public async Task<GetRegionByIdResponse> HandleAsync(GetRegionByIdQuery query, CancellationToken cancellationToken = default)
    {
        await _validator.ValidateAndThrowAsync(query, cancellationToken);

        var check_region = await _db.Regions.AnyAsync(x => x.Id == query.RegionId, cancellationToken);
        if(!check_region)
        {
            throw new UseCaseNotFoundException($"Регион с идентификатором {query.RegionId} не найден.");
        }

        var result = await _db.Regions
            .Where(x => x.Id == query.RegionId)
            .Select(x => new RegionDto(
                x.Id,
                x.Code,
                x.Name,
                x.IsActive,
                x.CreatedAt,
                x.UpdatedAt))
            .SingleAsync(cancellationToken);

        return new GetRegionByIdResponse { Data = result };
    }
}
