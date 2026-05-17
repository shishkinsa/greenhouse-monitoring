using GM.WebApi.UseCases.Handlers.Region.Commands.CreateRegion;
using GM.WebApi.UseCases.Handlers.Region.Commands.CreateRegion.Requests;
using GM.WebApi.UseCases.Handlers.Region.Commands.CreateRegion.Responses;
using GM.WebApi.UseCases.Handlers.Region.Commands.DeleteRegion;
using GM.WebApi.UseCases.Handlers.Region.Commands.PatchRegion;
using GM.WebApi.UseCases.Handlers.Region.Commands.PatchRegion.Requests;
using GM.WebApi.UseCases.Handlers.Region.DTOs;
using GM.WebApi.UseCases.Handlers.Region.Queries.GetRegionById;
using GM.WebApi.UseCases.Handlers.Region.Queries.GetRegionById.Responses;
using GM.WebApi.UseCases.Handlers.Region.Queries.ListRegions;
using GM.WebApi.UseCases.Handlers.Region.Queries.ListRegions.Responses;

using Microsoft.AspNetCore.Mvc;

using Requestum;

namespace GM.WebApi.WebApp.Controllers;

[Route("api/v1/regions")]
[ApiController]
public class RegionsController: ControllerBase
{
    private readonly IRequestum _requestum;

    public RegionsController(IRequestum requestum)
    {
        _requestum = requestum;
    }

    [HttpGet]
    public async Task<ActionResult<RegionListResponse>> GetListRegionsAsync(
        [FromQuery(Name = "page")] int page = 1,
        [FromQuery(Name = "page_size")] int pageSize = 25,
        CancellationToken cancellationToken = default)
    {
        var result = await _requestum.HandleAsync<ListRegionsQuery, RegionListResponse>(
            new ListRegionsQuery { Page = page, PageSize = pageSize },
            cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<CreateRegionResponse>> CreateRegionAsync([FromBody] CreateRegionRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await _requestum.ExecuteAsync<CreateRegionCommand, CreateRegionResponse>(
            new CreateRegionCommand { Code = request.Code, Name = request.Name });

        return Created($"/api/v1/regions/{result.Id}", result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GetRegionByIdResponse>> GetRegionByIdAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await _requestum.HandleAsync<GetRegionByIdQuery, GetRegionByIdResponse>(
            new GetRegionByIdQuery { RegionId = id },
            cancellationToken);

        return Ok(result);
    }

    [HttpPatch("{id:guid}")]
    public async Task<ActionResult<RegionDto>> PatchRegionAsync(
        [FromRoute] Guid id,
        [FromBody] PatchRegionRequest? request,
        CancellationToken cancellationToken = default)
    {
        if(request is null)
        {
            return BadRequest();
        }

        var result = await _requestum.ExecuteAsync<PatchRegionCommand, RegionDto>(
            new PatchRegionCommand
            {
                RegionId = id,
                Code = request.Code,
                Name = request.Name,
                IsActive = request.IsActive
            });

        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteRegionAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        await _requestum.ExecuteAsync(
            new DeleteRegionCommand { RegionId = id },
            cancellationToken);

        return NoContent();
    }
}
