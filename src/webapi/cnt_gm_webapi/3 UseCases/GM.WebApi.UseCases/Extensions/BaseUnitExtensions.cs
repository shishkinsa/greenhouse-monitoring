using System.Reflection;
using System.Runtime.Serialization;

using GM.WebApi.UseCases.Handlers.SensorTypes.Enums;

namespace GM.WebApi.UseCases.Extensions;

public static class BaseUnitExtensions
{
    public static string ToDefaultUnitString(this BaseUnit unit) =>
        GetEnumMemberValue(unit)
        ?? throw new ArgumentOutOfRangeException(nameof(unit), unit, "Base unit has no EnumMember value.");

    public static bool TryParseDefaultUnit(string? value, out BaseUnit unit)
    {
        unit = default;
        if(string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        var trimmed = value.Trim();
        foreach(var candidate in Enum.GetValues<BaseUnit>())
        {
            if(string.Equals(GetEnumMemberValue(candidate), trimmed, StringComparison.Ordinal))
            {
                unit = candidate;
                return true;
            }
        }

        return false;
    }

    private static string? GetEnumMemberValue(BaseUnit unit) =>
        typeof(BaseUnit)
            .GetField(unit.ToString())
            ?.GetCustomAttribute<EnumMemberAttribute>()
            ?.Value;
}
