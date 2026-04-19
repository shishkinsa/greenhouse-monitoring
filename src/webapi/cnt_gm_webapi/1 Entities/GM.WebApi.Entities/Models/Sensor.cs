using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GM.WebApi.Entities.Models;

/// <summary>
/// Экземпляр датчика (логический или физический узел измерения).
/// Прямая привязка к теплице в этой таблице отсутствует; связь с теплицей выводится через контроллер и периоды установок.
/// Таблица <c>app.sensors</c>.
/// </summary>
[Table("sensors", Schema = "app")]
public class Sensor
{
    /// <summary>Идентификатор экземпляра датчика (первичный ключ).</summary>
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    /// <summary>Идентификатор типа датчика (<c>sensor_types.id</c>).</summary>
    [Column("sensor_type_id")]
    public Guid SensorTypeId { get; set; }

    /// <summary>Имя для отображения в UI.</summary>
    [Column("display_name")]
    [MaxLength(256)]
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>Описание места установки (необязательно).</summary>
    [Column("install_position")]
    [MaxLength(128)]
    public string? InstallPosition { get; set; }

    /// <summary>Учётная активность записи.</summary>
    [Column("is_active")]
    public bool IsActive { get; set; }

    /// <summary>Момент создания записи (UTC).</summary>
    [Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>Момент последнего обновления записи (UTC).</summary>
    [Column("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; }
}
