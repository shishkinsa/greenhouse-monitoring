using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GM.WebApi.Entities.Models;

/// <summary>
/// Период подключения экземпляра датчика к экземпляру контроллера (канал в прошивке и MQTT).
/// Непересечение интервалов для одного датчика и уникальность активных ключей на контроллере контролируются на уровне приложения или БД.
/// Таблица <c>app.sensor_digital_controller_links</c>.
/// </summary>
[Table("sensor_digital_controller_links", Schema = "app")]
public class SensorDigitalControllerLink
{
    /// <summary>Идентификатор связи, удобен для аудита и порогов (первичный ключ).</summary>
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    /// <summary>Идентификатор датчика (<c>sensors.id</c>).</summary>
    [Column("sensor_id")]
    public Guid SensorId { get; set; }

    /// <summary>Идентификатор контроллера (<c>digital_controllers.id</c>).</summary>
    [Column("digital_controller_id")]
    public Guid DigitalControllerId { get; set; }

    /// <summary>Ключ канала или датчика на стороне контроллера (полезная нагрузка MQTT).</summary>
    [Column("external_sensor_key")]
    [MaxLength(128)]
    public string ExternalSensorKey { get; set; } = string.Empty;

    /// <summary>Дата начала действия подключения (включительно по политике домена).</summary>
    [Column("sdate")]
    public DateOnly Sdate { get; set; }

    /// <summary>Дата окончания; <see langword="null" /> — открытый интервал.</summary>
    [Column("edate")]
    public DateOnly? Edate { get; set; }

    /// <summary>Момент создания записи (UTC).</summary>
    [Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>Момент последнего обновления записи (UTC).</summary>
    [Column("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; }
}
