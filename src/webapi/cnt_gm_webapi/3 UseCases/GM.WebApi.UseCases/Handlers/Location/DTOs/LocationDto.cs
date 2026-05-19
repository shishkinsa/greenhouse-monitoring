namespace GM.WebApi.UseCases.Handlers.Location.DTOs;

/// <summary>
/// DTO локации (площадка в регионе) для ответов API <c>Location</c>.
/// </summary>
public record LocationDto(
    Guid Id,
    Guid RegionId,
    string Code,
    string Name,
    string? Address,
    decimal? Latitude,
    decimal? Longitude,
    bool IsActive,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);
