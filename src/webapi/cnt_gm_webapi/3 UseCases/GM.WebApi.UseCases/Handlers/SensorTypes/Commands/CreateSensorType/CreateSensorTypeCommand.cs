using GM.WebApi.UseCases.Handlers.SensorTypes.DTOs;

using Requestum.Contract;

namespace GM.WebApi.UseCases.Handlers.SensorTypes.Commands.CreateSensorType;

/// <summary>
/// Команда создания записи в справочнике <c>sensor_types</c>.
/// </summary>
public class CreateSensorTypeCommand : ICommand<SensorTypeDto>
{
    /// <summary>Уникальный код типа (<c>temperature</c>, <c>humidity</c>, <c>soil_ph</c>).</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Наименование типа для UI и отчётов.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Базовая единица измерения (<c>C</c>, <c>%</c>, <c>pH</c>); необязательно.</summary>
    public string? DefaultUnit { get; set; }

    /// <summary>Физически допустимый минимум показания; необязательно.</summary>
    public decimal? ValueMin { get; set; }

    /// <summary>Физически допустимый максимум показания; необязательно.</summary>
    public decimal? ValueMax { get; set; }
}
