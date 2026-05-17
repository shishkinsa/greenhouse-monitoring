using GM.WebApi.UseCases.Handlers.SensorTypes.DTOs;

namespace GM.WebApi.UseCases.Handlers.SensorTypes.Queries.GetSensorTypeById.Responses;

/// <summary>
/// Ответ API при получении типа датчика по идентификатору (согласован с OpenAPI <c>SensorType</c> в теле <see cref="Data"/>).
/// </summary>
public class GetSensorTypeByIdResponse
{
    /// <summary>Данные типа датчика.</summary>
    public SensorTypeDto Data { get; set; } = null!;
}
