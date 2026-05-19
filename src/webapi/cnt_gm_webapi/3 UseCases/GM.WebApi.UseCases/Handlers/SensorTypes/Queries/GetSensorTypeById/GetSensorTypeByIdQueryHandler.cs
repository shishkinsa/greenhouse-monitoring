using FluentValidation;

using GM.WebApi.Infrastructure.Interfaces.DataAccess;
using GM.WebApi.UseCases.Exceptions;
using GM.WebApi.UseCases.Handlers.SensorTypes.Mappings;
using GM.WebApi.UseCases.Handlers.SensorTypes.Queries.GetSensorTypeById.Responses;

using Microsoft.EntityFrameworkCore;

using Requestum.Contract;

namespace GM.WebApi.UseCases.Handlers.SensorTypes.Queries.GetSensorTypeById;

/// <summary>
/// Получение типа датчика по идентификатору из <c>app.sensor_types</c>.
/// </summary>
public sealed class GetSensorTypeByIdQueryHandler
    : IAsyncQueryHandler<GetSensorTypeByIdQuery, GetSensorTypeByIdResponse>
{
    private readonly IDbContext _db;
    private readonly IValidator<GetSensorTypeByIdQuery> _validator;

    public GetSensorTypeByIdQueryHandler(
        IDbContext db,
        IValidator<GetSensorTypeByIdQuery> validator)
    {
        _db = db;
        _validator = validator;
    }

    public async Task<GetSensorTypeByIdResponse> HandleAsync(
        GetSensorTypeByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        await _validator.ValidateAndThrowAsync(query, cancellationToken);

        var entity = await _db.SensorTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(st => st.Id == query.SensorTypeId, cancellationToken) ?? throw new UseCaseNotFoundException(
                $"Тип датчика с идентификатором {query.SensorTypeId} не найден.");

        return new GetSensorTypeByIdResponse
        {
            Data = SensorTypeMappings.ToDto(entity),
        };
    }
}
