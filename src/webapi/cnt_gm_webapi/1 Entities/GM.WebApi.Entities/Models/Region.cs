using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GM.WebApi.Entities.Models;

/// <summary>
/// Регион в иерархии локаций (родитель для площадок <see cref="Location" />).
/// Таблица <c>app.regions</c>.
/// </summary>
[Table("regions", Schema = "app")]
public class Region
{
    /// <summary>Идентификатор региона (первичный ключ).</summary>
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    /// <summary>Уникальный код региона.</summary>
    [Column("code")]
    [MaxLength(32)]
    public string Code { get; set; } = string.Empty;

    /// <summary>Отображаемое наименование региона.</summary>
    [Column("name")]
    [MaxLength(256)]
    public string Name { get; set; } = string.Empty;

    /// <summary>Признак активности записи в справочнике.</summary>
    [Column("is_active")]
    public bool IsActive { get; set; }

    /// <summary>Момент создания записи (UTC).</summary>
    [Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>Момент последнего обновления записи (UTC).</summary>
    [Column("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; }
}
