using FluentValidation;

using GM.WebApi.UseCases.Extensions;

namespace GM.WebApi.UseCases.Handlers.SensorTypes.Commands.CreateSensorType.Validators;

/// <summary>
/// Валидация команды создания типа датчика.
/// </summary>
public sealed class CreateSensorTypeCommandValidator : AbstractValidator<CreateSensorTypeCommand>
{
    public CreateSensorTypeCommandValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Code)
            .Must(c => !string.IsNullOrWhiteSpace(c))
            .WithMessage("Код типа датчика обязателен.")
            .Must(c => c.Trim().Length <= 64)
            .WithMessage("Код типа датчика не может быть длиннее 64 символов.")
            .Must(c => SensorCodeExtensions.TryParseSensorCode(c, out _))
            .WithMessage(
                "Код типа датчика должен быть одним из: temperature, humidity, soil_ph.");

        RuleFor(x => x.Name)
            .Must(n => !string.IsNullOrWhiteSpace(n))
            .WithMessage("Наименование типа датчика обязательно.")
            .Must(n => n.Trim().Length <= 256)
            .WithMessage("Наименование типа датчика не может быть длиннее 256 символов.");

        RuleFor(x => x.DefaultUnit)
            .Must(u => string.IsNullOrWhiteSpace(u) || u.Trim().Length <= 16)
            .WithMessage("Единица измерения не может быть длиннее 16 символов.")
            .Must(u => string.IsNullOrWhiteSpace(u) || BaseUnitExtensions.TryParseDefaultUnit(u, out _))
            .WithMessage("Единица измерения должна быть одной из: C, %, pH.");

        RuleFor(x => x)
            .Must(x => x.ValueMin is null || x.ValueMax is null || x.ValueMin <= x.ValueMax)
            .WithMessage("Минимальное значение не может быть больше максимального.");
    }
}
