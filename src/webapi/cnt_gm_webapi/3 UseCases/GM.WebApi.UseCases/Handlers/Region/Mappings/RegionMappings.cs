using GM.WebApi.UseCases.Handlers.Region.DTOs;

using RegionEntity = GM.WebApi.Entities.Models.Region;

namespace GM.WebApi.UseCases.Handlers.Region.Mappings;

/// <summary>
/// Преобразование сущности региона в DTO ответов API.
/// </summary>
public static class RegionMappings
{
    public static RegionDto ToDto(RegionEntity region) =>
        new(region.Id, region.Code, region.Name, region.IsActive, region.CreatedAt, region.UpdatedAt);
}
