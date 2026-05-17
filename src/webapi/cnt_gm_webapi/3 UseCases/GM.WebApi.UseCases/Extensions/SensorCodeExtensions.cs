using System.Reflection;
using System.Runtime.Serialization;

using GM.WebApi.UseCases.Handlers.SensorTypes.Enums;

namespace GM.WebApi.UseCases.Extensions;

public static class SensorCodeExtensions
{
    public static string ToCodeString(this SensorCode code) =>
        GetEnumMemberValue(code)
        ?? throw new ArgumentOutOfRangeException(nameof(code), code, "Sensor code has no EnumMember value.");

    public static bool TryParseSensorCode(string? value, out SensorCode code)
    {
        code = default;
        if (string.IsNullOrWhiteSpace(value))
            return false;

        var trimmed = value.Trim();
        foreach (var candidate in Enum.GetValues<SensorCode>())
        {
            if (string.Equals(GetEnumMemberValue(candidate), trimmed, StringComparison.OrdinalIgnoreCase))
            {
                code = candidate;
                return true;
            }
        }

        return false;
    }

    private static string? GetEnumMemberValue(SensorCode code) =>
        typeof(SensorCode)
            .GetField(code.ToString())
            ?.GetCustomAttribute<EnumMemberAttribute>()
            ?.Value;
}
