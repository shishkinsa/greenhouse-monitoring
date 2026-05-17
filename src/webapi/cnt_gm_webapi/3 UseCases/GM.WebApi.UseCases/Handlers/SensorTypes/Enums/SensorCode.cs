using System.Runtime.Serialization;

namespace GM.WebApi.UseCases.Handlers.SensorTypes.Enums;

/// <summary>
/// Уникальные коды типа датчиков (<c>sensor_types.code</c>).
/// </summary>
public enum SensorCode
{
    /// <summary>Температура.</summary>
    [EnumMember(Value = "temperature")]
    Temperature = 0,

    /// <summary>Влажность.</summary>
    [EnumMember(Value = "humidity")]
    Humidity,

    /// <summary>Кислотность почвы.</summary>
    [EnumMember(Value = "soil_ph")]
    SoilPh,
}
