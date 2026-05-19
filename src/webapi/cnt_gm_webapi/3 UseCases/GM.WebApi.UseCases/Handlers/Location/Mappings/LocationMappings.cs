using GM.WebApi.UseCases.Handlers.Location.DTOs;

using LocationEntity = GM.WebApi.Entities.Models.Location;

namespace GM.WebApi.UseCases.Handlers.Location.Mappings;

public static class LocationMappings
{
    public static LocationDto ToDto(LocationEntity entity) =>
        new(
            entity.Id,
            entity.RegionId,
            entity.Code,
            entity.Name,
            entity.Address,
            entity.Latitude,
            entity.Longitude,
            entity.IsActive,
            entity.CreatedAt,
            entity.UpdatedAt);
}
