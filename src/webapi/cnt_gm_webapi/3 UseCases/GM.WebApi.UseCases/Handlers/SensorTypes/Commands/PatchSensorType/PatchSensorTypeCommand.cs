using GM.WebApi.UseCases.Handlers.SensorTypes.DTOs;

using Requestum.Contract;

namespace GM.WebApi.UseCases.Handlers.SensorTypes.Commands.PatchSensorType;

/// <summary>
/// Частичное обновление типа датчика по идентификатору.
/// </summary>
public sealed class PatchSensorTypeCommand : ICommand<SensorTypeDto>
{
    /// <summary>Идентификатор типа датчика (<c>sensor_types.id</c>).</summary>
    public Guid SensorTypeId { get; set; }

    /// <summary>Новый код типа (если передан — заменяет текущий).</summary>
    public string? Code { get; set; }

    /// <summary>Новое наименование (если передано — заменяет текущее).</summary>
    public string? Name { get; set; }

    /// <summary>Новая базовая единица измерения (если передана — заменяет текущую).</summary>
    public string? DefaultUnit { get; set; }

    /// <summary>Новый минимум показания (если передан — заменяет текущий).</summary>
    public decimal? ValueMin { get; set; }

    /// <summary>Новый максимум показания (если передан — заменяет текущий).</summary>
    public decimal? ValueMax { get; set; }

    /// <summary>Новый признак активности (если передан — заменяет текущий).</summary>
    public bool? IsActive { get; set; }
}
