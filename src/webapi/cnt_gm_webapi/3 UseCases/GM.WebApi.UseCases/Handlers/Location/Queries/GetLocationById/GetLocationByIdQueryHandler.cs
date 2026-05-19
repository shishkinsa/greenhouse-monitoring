using FluentValidation;

using GM.WebApi.Infrastructure.Interfaces.DataAccess;
using GM.WebApi.UseCases.Exceptions;
using GM.WebApi.UseCases.Handlers.Location.DTOs;
using GM.WebApi.UseCases.Handlers.Location.Mappings;

using Microsoft.EntityFrameworkCore;

using Requestum.Contract;

namespace GM.WebApi.UseCases.Handlers.Location.Queries.GetLocationById;

public sealed class GetLocationByIdQueryHandler : IAsyncQueryHandler<GetLocationByIdQuery, LocationDto>
{
    private readonly IDbContext _db;
    private readonly IValidator<GetLocationByIdQuery> _validator;

    public GetLocationByIdQueryHandler(
        IDbContext db,
        IValidator<GetLocationByIdQuery> validator)
    {
        _db = db;
        _validator = validator;
    }

    public async Task<LocationDto> HandleAsync(
        GetLocationByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        await _validator.ValidateAndThrowAsync(query, cancellationToken);

        var entity = await _db.Locations
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.Id == query.LocationId, cancellationToken)
            ?? throw new UseCaseNotFoundException(
                $"Локация с идентификатором {query.LocationId} не найдена.");

        return LocationMappings.ToDto(entity);
    }
}
