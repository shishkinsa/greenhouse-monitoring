using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GM.WebApi.Entities.Models;

/// <summary>
/// Площадка / локация внутри региона (адрес, координаты).
/// К теплицам привязка через <see cref="Greenhouse.LocationId" />.
/// Таблица <c>app.locations</c>.
/// </summary>
[Table("locations", Schema = "app")]
public class Location
{
    /// <summary>Идентификатор локации (первичный ключ).</summary>
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    /// <summary>Идентификатор региона (<c>regions.id</c>).</summary>
    [Column("region_id")]
    public Guid RegionId { get; set; }

    /// <summary>Код локации; уникален в паре с регионом.</summary>
    [Column("code")]
    [MaxLength(32)]
    public string Code { get; set; } = string.Empty;

    /// <summary>Наименование площадки или объекта.</summary>
    [Column("name")]
    [MaxLength(256)]
    public string Name { get; set; } = string.Empty;

    /// <summary>Почтовый или произвольный адрес.</summary>
    [Column("address")]
    [MaxLength(512)]
    public string? Address { get; set; }

    /// <summary>Широта (градусы), <c>numeric(9,6)</c>.</summary>
    [Column("latitude")]
    public decimal? Latitude { get; set; }

    /// <summary>Долгота (градусы), <c>numeric(9,6)</c>.</summary>
    [Column("longitude")]
    public decimal? Longitude { get; set; }

    /// <summary>Признак активности записи.</summary>
    [Column("is_active")]
    public bool IsActive { get; set; }

    /// <summary>Момент создания записи (UTC).</summary>
    [Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>Момент последнего обновления записи (UTC).</summary>
    [Column("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; }
}
