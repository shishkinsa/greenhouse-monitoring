using GM.WebApi.UseCases.Handlers.Greenhouse.DTOs;
using GM.WebApi.Utils.DTOs;

namespace GM.WebApi.UseCases.Handlers.Greenhouse.Queries.ListGreenhousesByOrganization.Responses;

/// <summary>Страница теплиц организации (OpenAPI <c>PagedGreenhouses</c>).</summary>
public class GreenhouseListResponse : PageInfo
{
    public IReadOnlyList<GreenhouseDto> Items { get; init; } = Array.Empty<GreenhouseDto>();
}
