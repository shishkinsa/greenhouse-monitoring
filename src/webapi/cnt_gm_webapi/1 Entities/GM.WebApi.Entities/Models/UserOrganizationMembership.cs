using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GM.WebApi.Entities.Models;

/// <summary>
/// Связь учётной записи из Identity с организацией.
/// <c>user_id</c> логически соответствует <c>sub</c> в токене и PK в Identity; прямой FK между БД не используется.
/// Таблица <c>app.user_organization_memberships</c>.
/// </summary>
[Table("user_organization_memberships", Schema = "app")]
public class UserOrganizationMembership
{
    /// <summary>Суррогатный идентификатор строки членства.</summary>
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    /// <summary>Идентификатор пользователя в Identity.</summary>
    [Column("user_id")]
    public Guid UserId { get; set; }

    /// <summary>Идентификатор организации (<c>organizations.id</c>).</summary>
    [Column("organization_id")]
    public Guid OrganizationId { get; set; }

    /// <summary>Основная организация для UI (не более одной активной на пользователя — правила на стороне приложения).</summary>
    [Column("is_primary")]
    public bool IsPrimary { get; set; }

    /// <summary>Должность или роль в оргструктуре отображения.</summary>
    [Column("title")]
    [MaxLength(256)]
    public string? Title { get; set; }

    /// <summary>Роль в организации (например <c>org_admin</c>, <c>member</c>).</summary>
    [Column("membership_role")]
    [MaxLength(64)]
    public string MembershipRole { get; set; } = string.Empty;

    /// <summary>Статус членства: <c>active</c>, <c>invited</c>, <c>suspended</c> и т.д.</summary>
    [Column("status")]
    [MaxLength(32)]
    public string Status { get; set; } = string.Empty;

    /// <summary>Начало членства (UTC).</summary>
    [Column("joined_at")]
    public DateTimeOffset JoinedAt { get; set; }

    /// <summary>Окончание членства (UTC); <see langword="null" /> — действующее членство.</summary>
    [Column("left_at")]
    public DateTimeOffset? LeftAt { get; set; }

    /// <summary>Момент создания записи (UTC).</summary>
    [Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>Момент последнего обновления записи (UTC).</summary>
    [Column("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; }
}
