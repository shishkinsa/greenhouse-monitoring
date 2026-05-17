namespace GM.WebApi.UseCases.Handlers.SensorTypes.DTOs;

/// <summary>
/// DTO типа датчика для ответов API (согласован с OpenAPI <c>SensorType</c>).
/// </summary>
public class SensorTypeDto
{
    /// <summary>Идентификатор типа датчика (первичный ключ).</summary>
    public Guid Id { get; set; }

    /// <summary>Уникальный код типа датчика (например <c>temperature</c>, <c>humidity</c>, <c>soil_ph</c>).</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Наименование типа для UI и отчётов.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Базовая единица измерения (<c>C</c>, <c>%</c>, <c>pH</c> и т.д.).</summary>
    public string? DefaultUnit { get; set; }

    /// <summary>Физически допустимый минимум показания.</summary>
    public decimal? ValueMin { get; set; }

    /// <summary>Физически допустимый максимум показания.</summary>
    public decimal? ValueMax { get; set; }

    /// <summary>Признак активности типа в справочнике (<c>true</c> — активен).</summary>
    public bool IsActive { get; set; }

    /// <summary>Момент создания записи (UTC).</summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>Момент последнего обновления записи (UTC).</summary>
    public DateTimeOffset UpdatedAt { get; set; }
}
