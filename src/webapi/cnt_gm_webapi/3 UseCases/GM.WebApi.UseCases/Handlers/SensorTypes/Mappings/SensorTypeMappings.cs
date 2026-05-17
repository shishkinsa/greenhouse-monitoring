using GM.WebApi.UseCases.Extensions;
using GM.WebApi.UseCases.Handlers.SensorTypes.DTOs;

using SensorTypeEntity = GM.WebApi.Entities.Models.SensorType;

namespace GM.WebApi.UseCases.Handlers.SensorTypes.Mappings;

/// <summary>
/// Преобразование сущности типа датчика в DTO ответов API.
/// </summary>
public static class SensorTypeMappings
{
    public static SensorTypeDto ToDto(SensorTypeEntity entity)
    {
        if (!SensorCodeExtensions.TryParseSensorCode(entity.Code, out var code))
        {
            throw new InvalidOperationException(
                $"Неизвестный код типа датчика в БД: '{entity.Code}'.");
        }

        if (entity.DefaultUnit is not null
            && !BaseUnitExtensions.TryParseDefaultUnit(entity.DefaultUnit, out _))
        {
            throw new InvalidOperationException(
                $"Неизвестная единица измерения в БД: '{entity.DefaultUnit}'.");
        }

        return new SensorTypeDto
        {
            Id = entity.Id,
            Code = code.ToCodeString(),
            Name = entity.Name,
            DefaultUnit = entity.DefaultUnit,
            ValueMin = entity.ValueMin,
            ValueMax = entity.ValueMax,
            IsActive = entity.IsActive,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
        };
    }
}
