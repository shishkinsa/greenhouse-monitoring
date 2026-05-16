using GM.WebApi.UseCases.Handlers.Greenhouse.Queries.ListGreenhousesByOrganization;
using GM.WebApi.UseCases.Handlers.Greenhouse.Queries.ListGreenhousesByOrganization.Responses;

using Microsoft.AspNetCore.Mvc;

using Requestum;

namespace GM.WebApi.WebApp.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class OrganisationsController : ControllerBase
{
    private readonly IRequestum _requestum;

    public OrganisationsController(IRequestum requestum)
    {
        _requestum = requestum;
    }

    /// <summary>
    /// Теплицы организации (пагинация). Совпадает с OpenAPI <c>GET /api/v1/organisations/{id}/greenhouses</c>.
    /// </summary>
    [HttpGet("{id:guid}/greenhouses")]
    public async Task<ActionResult<GreenhouseListResponse>> ListGreenhousesByOrganisationAsync(
        [FromRoute] Guid id,
        [FromQuery(Name = "page")] int page = 1,
        [FromQuery(Name = "page_size")] int pageSize = 25,
        CancellationToken cancellationToken = default)
    {
        var result = await _requestum.HandleAsync<ListGreenhousesByOrganizationQuery, GreenhouseListResponse>(
            new ListGreenhousesByOrganizationQuery
            {
                OrganizationId = id,
                Page = page,
                PageSize = pageSize
            },
            cancellationToken);

        return Ok(result);
    }
}
