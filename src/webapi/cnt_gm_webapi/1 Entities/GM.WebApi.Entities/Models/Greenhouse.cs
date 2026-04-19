using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GM.WebApi.Entities.Models;

/// <summary>
/// Теплица: принадлежит организации и расположена на площадке (<see cref="Location" />).
/// Доступ пользователей к теплицам через членство в организации.
/// Таблица <c>app.greenhouses</c>.
/// </summary>
[Table("greenhouses", Schema = "app")]
public class Greenhouse
{
    /// <summary>Идентификатор теплицы (первичный ключ).</summary>
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    /// <summary>Владелец или арендатор (<c>organizations.id</c>).</summary>
    [Column("organization_id")]
    public Guid OrganizationId { get; set; }

    /// <summary>Площадка размещения (<c>locations.id</c>).</summary>
    [Column("location_id")]
    public Guid LocationId { get; set; }

    /// <summary>Код теплицы; уникален в пределах организации.</summary>
    [Column("code")]
    [MaxLength(32)]
    public string Code { get; set; } = string.Empty;

    /// <summary>Отображаемое имя.</summary>
    [Column("name")]
    [MaxLength(256)]
    public string Name { get; set; } = string.Empty;

    /// <summary>Площадь в м²; необязательное поле.</summary>
    [Column("area_m2")]
    public decimal? AreaM2 { get; set; }

    /// <summary>Часовой пояс (например <c>Europe/Moscow</c>).</summary>
    [Column("timezone")]
    [MaxLength(64)]
    public string? Timezone { get; set; }

    /// <summary>Признак активности объекта.</summary>
    [Column("is_active")]
    public bool IsActive { get; set; }

    /// <summary>Дата ввода теплицы в эксплуатацию (календарная дата).</summary>
    [Column("commissioned_at")]
    public DateOnly? CommissionedAt { get; set; }

    /// <summary>Момент создания записи (UTC).</summary>
    [Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>Момент последнего обновления записи (UTC).</summary>
    [Column("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; }
}
