using GM.WebApi.UseCases.Handlers.Location.DTOs;
using GM.WebApi.Utils.DTOs;

namespace GM.WebApi.UseCases.Handlers.Location.Queries.ListLocations.Responses;

/// <summary>
/// Страница локаций; поля пагинации — <see cref="PageInfo" /> (совпадает с OpenAPI <c>PagedLocations</c>).
/// </summary>
public class LocationListResponse: PageInfo
{
    public IReadOnlyList<LocationDto> Items { get; init; } = Array.Empty<LocationDto>();
}
