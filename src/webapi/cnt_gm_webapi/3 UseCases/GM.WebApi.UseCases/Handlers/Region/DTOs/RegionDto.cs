namespace GM.WebApi.UseCases.Handlers.Region.DTOs;


/// <summary>
/// DTO региона (географическая/административная единица) для справочника регионов в системе Greenhouse Monitoring.
/// Используется в ответах API при получении списка регионов.
/// </summary>
public record RegionDto(
    /// <summary>
    /// Уникальный идентификатор региона (UUID)
    /// </summary>
    Guid Id,

    /// <summary>
    /// Уникальный код региона
    /// </summary>
    string Code,

    /// <summary>
    /// Наименование региона
    /// </summary>
    string Name,

    /// <summary>
    /// Признак активности региона (true — активен)
    /// </summary>
    bool IsActive,

    /// <summary>
    /// Дата и время создания записи
    /// </summary>
    DateTimeOffset CreatedAt,

    /// <summary>
    /// Дата и время последнего обновления записи
    /// </summary>
    DateTimeOffset UpdatedAt
);

