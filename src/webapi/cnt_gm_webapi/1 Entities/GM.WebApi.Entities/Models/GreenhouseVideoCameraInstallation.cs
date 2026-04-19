using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GM.WebApi.Entities.Models;

/// <summary>
/// Период установки видеокамеры на теплицу.
/// Границы <see cref="Sdate" /> и <see cref="Edate" /> задают интервал; <see cref="Edate" /> со значением <see langword="null" /> — открытый интервал.
/// Таблица <c>app.greenhouse_video_camera_installations</c>.
/// </summary>
[Table("greenhouse_video_camera_installations", Schema = "app")]
public class GreenhouseVideoCameraInstallation
{
    /// <summary>Идентификатор строки установки (первичный ключ).</summary>
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    /// <summary>Идентификатор камеры (<c>video_cameras.id</c>).</summary>
    [Column("video_camera_id")]
    public Guid VideoCameraId { get; set; }

    /// <summary>Идентификатор теплицы (<c>greenhouses.id</c>).</summary>
    [Column("greenhouse_id")]
    public Guid GreenhouseId { get; set; }

    /// <summary>Дата начала действия установки камеры на теплице.</summary>
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
