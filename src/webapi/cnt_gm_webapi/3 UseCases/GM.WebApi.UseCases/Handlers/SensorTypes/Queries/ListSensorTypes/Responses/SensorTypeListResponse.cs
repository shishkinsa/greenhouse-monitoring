using GM.WebApi.UseCases.Handlers.SensorTypes.DTOs;
using GM.WebApi.Utils.DTOs;

namespace GM.WebApi.UseCases.Handlers.SensorTypes.Queries.ListSensorTypes.Responses;

/// <summary>
/// Страница типов датчиков; поля пагинации — <see cref="PageInfo" /> (совпадает с OpenAPI <c>PagedSensorTypes</c>).
/// </summary>
public class SensorTypeListResponse : PageInfo
{
    public IReadOnlyList<SensorTypeDto> Items { get; init; } = Array.Empty<SensorTypeDto>();
}
