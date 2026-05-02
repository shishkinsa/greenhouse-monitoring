using GM.WebApi.UseCases.Handlers.Region.DTOs;

namespace GM.WebApi.UseCases.Handlers.Region.Queries.GetRegionById.Responses;

/// <summary>
/// Ответ API при получении региона по идентификатору.
/// </summary>
public class GetRegionByIdResponse
{
    /// <summary>
    /// Данные региона.
    /// </summary>
    public RegionDto Data { get; set; } = null!;
}
