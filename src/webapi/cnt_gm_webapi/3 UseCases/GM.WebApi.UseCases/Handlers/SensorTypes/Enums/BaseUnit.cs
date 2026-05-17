using System.Runtime.Serialization;

namespace GM.WebApi.UseCases.Handlers.SensorTypes.Enums;

/// <summary>
/// Базовые единицы измерения (<c>sensor_types.default_unit</c>).
/// </summary>
public enum BaseUnit
{
    [EnumMember(Value = "C")]
    Celsius = 0,

    [EnumMember(Value = "%")]
    Percent,

    [EnumMember(Value = "pH")]
    Ph,
}
