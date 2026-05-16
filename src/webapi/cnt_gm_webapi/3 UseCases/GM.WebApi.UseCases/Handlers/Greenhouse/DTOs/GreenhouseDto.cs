namespace GM.WebApi.UseCases.Handlers.Greenhouse.DTOs;

/// <summary>
/// DTO теплицы для ответов API (согласован с OpenAPI <c>Greenhouse</c>).
/// </summary>
public record GreenhouseDto(
    Guid Id,
    string Code,
    string Name,
    Guid OrganisationId,
    Guid LocationId,
    decimal? AreaM2,
    string? Timezone,
    bool IsActive,
    DateOnly? CommissionedAt,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt,
    string? Address);
