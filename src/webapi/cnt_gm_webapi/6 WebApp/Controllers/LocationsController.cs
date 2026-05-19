using GM.WebApi.UseCases.Handlers.Location.Commands.CreateLocation;
using GM.WebApi.UseCases.Handlers.Location.Commands.CreateLocation.Requests;
using GM.WebApi.UseCases.Handlers.Location.Commands.DeleteLocation;
using GM.WebApi.UseCases.Handlers.Location.Commands.PatchLocation;
using GM.WebApi.UseCases.Handlers.Location.Commands.PatchLocation.Requests;
using GM.WebApi.UseCases.Handlers.Location.DTOs;
using GM.WebApi.UseCases.Handlers.Location.Queries.GetLocationById;
using GM.WebApi.UseCases.Handlers.Location.Queries.ListLocations;
using GM.WebApi.UseCases.Handlers.Location.Queries.ListLocations.Responses;

using Microsoft.AspNetCore.Mvc;

using Requestum;

namespace GM.WebApi.WebApp.Controllers;

[Route("api/v1/locations")]
[ApiController]
public class LocationsController: ControllerBase
{
    private readonly IRequestum _requestum;

    public LocationsController(IRequestum requestum) => _requestum = requestum;

    [HttpGet]
    public async Task<ActionResult<LocationListResponse>> ListLocationsAsync(
        [FromQuery(Name = "page")] int page = 1,
        [FromQuery(Name = "page_size")] int pageSize = 25,
        CancellationToken cancellationToken = default)
    {
        var result = await _requestum.HandleAsync<ListLocationsQuery, LocationListResponse>(
            new ListLocationsQuery { Page = page, PageSize = pageSize },
            cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<LocationDto>> CreateLocationAsync(
        [FromBody] CreateLocationRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await _requestum.ExecuteAsync<CreateLocationCommand, LocationDto>(
            new CreateLocationCommand
            {
                RegionId = request.RegionId,
                Code = request.Code,
                Name = request.Name,
                Address = request.Address,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
            });

        return Created($"/api/v1/locations/{result.Id}", result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<LocationDto>> GetLocationByIdAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await _requestum.HandleAsync<GetLocationByIdQuery, LocationDto>(
            new GetLocationByIdQuery { LocationId = id },
            cancellationToken);

        return Ok(result);
    }

    [HttpPatch("{id:guid}")]
    public async Task<ActionResult<LocationDto>> PatchLocationAsync(
        [FromRoute] Guid id,
        [FromBody] PatchLocationRequest? request,
        CancellationToken cancellationToken = default)
    {
        if(request is null)
        {
            return BadRequest();
        }

        var result = await _requestum.ExecuteAsync<PatchLocationCommand, LocationDto>(
            new PatchLocationCommand
            {
                LocationId = id,
                RegionId = request.RegionId,
                Code = request.Code,
                Name = request.Name,
                Address = request.Address,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                IsActive = request.IsActive,
            });

        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteLocationAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        await _requestum.ExecuteAsync(
            new DeleteLocationCommand { LocationId = id },
            cancellationToken);

        return NoContent();
    }
}
