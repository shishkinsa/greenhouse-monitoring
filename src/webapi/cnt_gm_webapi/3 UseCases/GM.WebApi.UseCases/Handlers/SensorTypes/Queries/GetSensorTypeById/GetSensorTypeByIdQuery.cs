using GM.WebApi.UseCases.Handlers.SensorTypes.Queries.GetSensorTypeById.Responses;

using Requestum.Contract;

namespace GM.WebApi.UseCases.Handlers.SensorTypes.Queries.GetSensorTypeById;

/// <summary>
/// Запрос типа датчика по идентификатору.
/// </summary>
public class GetSensorTypeByIdQuery : IQuery<GetSensorTypeByIdResponse>
{
    /// <summary>Идентификатор типа датчика (<c>sensor_types.id</c>).</summary>
    public Guid SensorTypeId { get; set; }
}
