using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GM.WebApi.Entities.Models;

/// <summary>
/// Справочник типов датчиков (физические величины, допустимые диапазоны, единицы).
/// Таблица <c>app.sensor_types</c>.
/// </summary>
[Table("sensor_types", Schema = "app")]
public class SensorType
{
    /// <summary>Идентификатор типа датчика (первичный ключ).</summary>
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    /// <summary>Уникальный код типа (например <c>temperature</c>, <c>humidity</c>, <c>soil_ph</c>).</summary>
    [Column("code")]
    [MaxLength(64)]
    public string Code { get; set; } = string.Empty;

    /// <summary>Наименование типа для UI и отчётов.</summary>
    [Column("name")]
    [MaxLength(256)]
    public string Name { get; set; } = string.Empty;

    /// <summary>Базовая единица измерения (<c>C</c>, <c>%</c>, <c>pH</c> и т.д.).</summary>
    [Column("default_unit")]
    [MaxLength(16)]
    public string? DefaultUnit { get; set; }

    /// <summary>Физически допустимый минимум показания.</summary>
    [Column("value_min")]
    public decimal? ValueMin { get; set; }

    /// <summary>Физически допустимый максимум показания.</summary>
    [Column("value_max")]
    public decimal? ValueMax { get; set; }

    /// <summary>Признак активности типа в справочнике.</summary>
    [Column("is_active")]
    public bool IsActive { get; set; }

    /// <summary>Момент создания записи (UTC).</summary>
    [Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>Момент последнего обновления записи (UTC).</summary>
    [Column("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; }
}
