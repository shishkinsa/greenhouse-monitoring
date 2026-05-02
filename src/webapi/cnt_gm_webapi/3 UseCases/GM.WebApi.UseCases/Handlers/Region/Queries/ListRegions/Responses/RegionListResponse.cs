using GM.WebApi.UseCases.Handlers.Region.DTOs;
using GM.WebApi.Utils.DTOs;

namespace GM.WebApi.UseCases.Handlers.Region.Queries.ListRegions.Responses;

/// <summary>
/// Страница регионов; поля пагинации — <see cref="PageInfo" /> (совпадает с OpenAPI <c>PagedRegions</c>).
/// </summary>
public class RegionListResponse : PageInfo
{
    public IReadOnlyList<RegionDto> Items { get; init; } = Array.Empty<RegionDto>();
}
