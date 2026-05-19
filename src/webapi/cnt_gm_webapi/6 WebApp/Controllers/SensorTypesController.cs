using GM.WebApi.UseCases.Handlers.SensorTypes.Commands.CreateSensorType;
using GM.WebApi.UseCases.Handlers.SensorTypes.Commands.CreateSensorType.Requests;
using GM.WebApi.UseCases.Handlers.SensorTypes.Commands.DeleteSensorType;
using GM.WebApi.UseCases.Handlers.SensorTypes.Commands.PatchSensorType;
using GM.WebApi.UseCases.Handlers.SensorTypes.Commands.PatchSensorType.Requests;
using GM.WebApi.UseCases.Handlers.SensorTypes.DTOs;
using GM.WebApi.UseCases.Handlers.SensorTypes.Queries.GetSensorTypeById;
using GM.WebApi.UseCases.Handlers.SensorTypes.Queries.GetSensorTypeById.Responses;
using GM.WebApi.UseCases.Handlers.SensorTypes.Queries.ListSensorTypes;
using GM.WebApi.UseCases.Handlers.SensorTypes.Queries.ListSensorTypes.Responses;

using Microsoft.AspNetCore.Mvc;

using Requestum;

namespace GM.WebApi.WebApp.Controllers;

[Route("api/v1/sensor-types")]
[ApiController]
public class SensorTypesController: ControllerBase
{
    private readonly IRequestum _requestum;

    public SensorTypesController(IRequestum requestum) => _requestum = requestum;

    [HttpGet]
    public async Task<ActionResult<SensorTypeListResponse>> GetListSensorTypesAsync(
        [FromQuery(Name = "page")] int page = 1,
        [FromQuery(Name = "page_size")] int pageSize = 25,
        CancellationToken cancellationToken = default)
    {
        var result = await _requestum.HandleAsync<ListSensorTypesQuery, SensorTypeListResponse>(
            new ListSensorTypesQuery { Page = page, PageSize = pageSize },
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GetSensorTypeByIdResponse>> GetSensorTypeByIdAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await _requestum.HandleAsync<GetSensorTypeByIdQuery, GetSensorTypeByIdResponse>(
            new GetSensorTypeByIdQuery { SensorTypeId = id },
            cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<SensorTypeDto>> CreateSensorTypeAsync(
        [FromBody] CreateSensorTypeRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await _requestum.ExecuteAsync<CreateSensorTypeCommand, SensorTypeDto>(
            new CreateSensorTypeCommand
            {
                Code = request.Code,
                Name = request.Name,
                DefaultUnit = request.DefaultUnit,
                ValueMin = request.ValueMin,
                ValueMax = request.ValueMax,
            });

        return Created($"/api/v1/sensor-types/{result.Id}", result);
    }

    [HttpPatch("{id:guid}")]
    public async Task<ActionResult<SensorTypeDto>> PatchSensorTypeAsync(
        [FromRoute] Guid id,
        [FromBody] PatchSensorTypeRequest? request,
        CancellationToken cancellationToken = default)
    {
        if(request is null)
        {
            return BadRequest();
        }

        var result = await _requestum.ExecuteAsync<PatchSensorTypeCommand, SensorTypeDto>(
            new PatchSensorTypeCommand
            {
                SensorTypeId = id,
                Code = request.Code,
                Name = request.Name,
                DefaultUnit = request.DefaultUnit,
                ValueMin = request.ValueMin,
                ValueMax = request.ValueMax,
                IsActive = request.IsActive,
            });

        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteSensorTypeAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        await _requestum.ExecuteAsync(
            new DeleteSensorTypeCommand { SensorTypeId = id },
            cancellationToken);

        return NoContent();
    }
}
