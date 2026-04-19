using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GM.WebApi.Entities.Models;

/// <summary>
/// Период установки контроллера на теплицу: с какой даты обслуживается объект.
/// Границы <see cref="Sdate" /> и <see cref="Edate" /> задают интервал; <see cref="Edate" /> со значением <see langword="null" /> — открытый интервал.
/// Таблица <c>app.greenhouse_digital_controller_installations</c>.
/// </summary>
[Table("greenhouse_digital_controller_installations", Schema = "app")]
public class GreenhouseDigitalControllerInstallation
{
    /// <summary>Идентификатор строки установки (первичный ключ).</summary>
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    /// <summary>Идентификатор контроллера (<c>digital_controllers.id</c>).</summary>
    [Column("digital_controller_id")]
    public Guid DigitalControllerId { get; set; }

    /// <summary>Идентификатор теплицы (<c>greenhouses.id</c>).</summary>
    [Column("greenhouse_id")]
    public Guid GreenhouseId { get; set; }

    /// <summary>Дата начала действия привязки контроллера к теплице (включительно по политике домена).</summary>
    [Column("sdate")]
    public DateOnly Sdate { get; set; }

    /// <summary>Дата окончания действия привязки; <see langword="null" /> — интервал без заранее заданного окончания.</summary>
    [Column("edate")]
    public DateOnly? Edate { get; set; }

    /// <summary>Момент создания записи (UTC).</summary>
    [Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>Момент последнего обновления записи (UTC).</summary>
    [Column("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; }
}
