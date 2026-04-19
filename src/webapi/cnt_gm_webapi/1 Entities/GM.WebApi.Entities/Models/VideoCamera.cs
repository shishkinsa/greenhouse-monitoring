using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GM.WebApi.Entities.Models;

/// <summary>
/// Экземпляр IP-видеокамеры (источник RTSP и интеграций вроде go2rtc).
/// Привязка к теплице только через <see cref="GreenhouseVideoCameraInstallation" />.
/// Таблица <c>app.video_cameras</c>.
/// </summary>
[Table("video_cameras", Schema = "app")]
public class VideoCamera
{
    /// <summary>Идентификатор камеры (первичный ключ).</summary>
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    /// <summary>Уникальный код камеры в интеграции и UI.</summary>
    [Column("camera_code")]
    [MaxLength(64)]
    public string CameraCode { get; set; } = string.Empty;

    /// <summary>Наименование камеры.</summary>
    [Column("name")]
    [MaxLength(256)]
    public string Name { get; set; } = string.Empty;

    /// <summary>Профиль потока по умолчанию (<c>main</c>, <c>sub</c> и т.п.).</summary>
    [Column("stream_profile")]
    [MaxLength(64)]
    public string? StreamProfile { get; set; }

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
