using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GM.WebApi.Entities.Models;

/// <summary>
/// Справочник организаций (юридические и операционные единицы).
/// Иерархия задаётся опционально: ссылка на родительскую организацию.
/// Таблица <c>app.organizations</c>.
/// </summary>
[Table("organizations", Schema = "app")]
public class Organization
{
    /// <summary>Идентификатор организации (первичный ключ).</summary>
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    /// <summary>Уникальный короткий код (интеграции, UI).</summary>
    [Column("code")]
    [MaxLength(64)]
    public string Code { get; set; } = string.Empty;

    /// <summary>Наименование организации.</summary>
    [Column("name")]
    [MaxLength(512)]
    public string Name { get; set; } = string.Empty;

    /// <summary>Идентификатор родительской организации; <see langword="null" /> — корень иерархии.</summary>
    [Column("parent_id")]
    public Guid? ParentId { get; set; }

    /// <summary>Произвольное текстовое описание.</summary>
    [Column("description")]
    public string? Description { get; set; }

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
