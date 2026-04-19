using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GM.WebApi.Entities.Models;

/// <summary>
/// Правило порога для генерации событий аналитики по показаниям датчика.
/// Таблица <c>app.sensor_threshold_rules</c>.
/// </summary>
[Table("sensor_threshold_rules", Schema = "app")]
public class SensorThresholdRule
{
    /// <summary>Идентификатор правила (первичный ключ).</summary>
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    /// <summary>Идентификатор экземпляра датчика (<c>sensors.id</c>).</summary>
    [Column("sensor_id")]
    public Guid SensorId { get; set; }

    /// <summary>Код правила (например <c>high_temp</c>, <c>low_humidity</c>).</summary>
    [Column("rule_code")]
    [MaxLength(64)]
    public string RuleCode { get; set; } = string.Empty;

    /// <summary>Оператор сравнения: <c>&gt;</c>, <c>&gt;=</c>, <c>&lt;</c>, <c>&lt;=</c>, <c>between</c> и т.д.</summary>
    [Column("operator")]
    [MaxLength(8)]
    public string Operator { get; set; } = string.Empty;

    /// <summary>Нижняя граница порога (если применимо к оператору).</summary>
    [Column("threshold_min")]
    public decimal? ThresholdMin { get; set; }

    /// <summary>Верхняя граница порога (если применимо к оператору).</summary>
    [Column("threshold_max")]
    public decimal? ThresholdMax { get; set; }

    /// <summary>Критичность события: <c>info</c>, <c>warning</c>, <c>critical</c>.</summary>
    [Column("severity")]
    [MaxLength(16)]
    public string Severity { get; set; } = string.Empty;

    /// <summary>Включено ли правило для оценки.</summary>
    [Column("is_enabled")]
    public bool IsEnabled { get; set; }

    /// <summary>Начало действия правила (UTC).</summary>
    [Column("effective_from")]
    public DateTimeOffset EffectiveFrom { get; set; }

    /// <summary>Окончание действия правила (UTC); <see langword="null" /> — без заранее заданного конца.</summary>
    [Column("effective_to")]
    public DateTimeOffset? EffectiveTo { get; set; }

    /// <summary>Момент создания записи (UTC).</summary>
    [Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>Момент последнего обновления записи (UTC).</summary>
    [Column("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; }
}
