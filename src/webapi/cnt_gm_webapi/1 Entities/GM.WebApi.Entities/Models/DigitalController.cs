using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GM.WebApi.Entities.Models;

/// <summary>
/// Экземпляр цифрового контроллера на объекте (edge), публикующего телеметрию по MQTT.
/// Привязка к теплице и датчикам задаётся таблицами периодов в разделе установок и связей.
/// Таблица <c>app.digital_controllers</c>.
/// </summary>
[Table("digital_controllers", Schema = "app")]
public class DigitalController
{
    /// <summary>Идентификатор экземпляра контроллера (первичный ключ).</summary>
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    /// <summary>Внутренний код устройства для интеграций и UI.</summary>
    [Column("code")]
    [MaxLength(64)]
    public string Code { get; set; } = string.Empty;

    /// <summary>Отображаемое имя контроллера.</summary>
    [Column("display_name")]
    [MaxLength(256)]
    public string DisplayName { get; set; } = string.Empty;

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
