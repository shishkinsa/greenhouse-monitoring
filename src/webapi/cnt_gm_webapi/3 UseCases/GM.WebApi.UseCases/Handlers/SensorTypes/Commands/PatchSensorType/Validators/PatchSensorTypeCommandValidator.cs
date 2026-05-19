using FluentValidation;

using GM.WebApi.UseCases.Extensions;

namespace GM.WebApi.UseCases.Handlers.SensorTypes.Commands.PatchSensorType.Validators;

/// <summary>
/// Валидация команды частичного обновления типа датчика.
/// </summary>
public sealed class PatchSensorTypeCommandValidator: AbstractValidator<PatchSensorTypeCommand>
{
    public PatchSensorTypeCommandValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.SensorTypeId)
            .NotEqual(Guid.Empty)
            .WithMessage("Идентификатор типа датчика не должен быть пустым.");

        RuleFor(x => x)
            .Must(c => c.Code != null || c.Name != null || c.DefaultUnit != null
                || c.ValueMin.HasValue || c.ValueMax.HasValue || c.IsActive.HasValue)
            .WithMessage(
                "Укажите хотя бы одно поле: code, name, defaultUnit, valueMin, valueMax или isActive.");

        When(x => x.Code != null, () =>
        {
            RuleFor(x => x.Code!)
                .Must(s => !string.IsNullOrWhiteSpace(s))
                .WithMessage("Код типа датчика не может быть пустым.")
                .Must(s => s.Trim().Length <= 64)
                .WithMessage("Код типа датчика не может быть длиннее 64 символов.")
                .Must(s => SensorCodeExtensions.TryParseSensorCode(s, out _))
                .WithMessage(
                    "Код типа датчика должен быть одним из: temperature, humidity, soil_ph.");
        });

        When(x => x.Name != null, () =>
        {
            RuleFor(x => x.Name!)
                .Must(s => !string.IsNullOrWhiteSpace(s))
                .WithMessage("Наименование типа датчика не может быть пустым.")
                .Must(s => s.Trim().Length <= 256)
                .WithMessage("Наименование типа датчика не может быть длиннее 256 символов.");
        });

        When(x => x.DefaultUnit != null, () =>
        {
            RuleFor(x => x.DefaultUnit!)
                .Must(s => string.IsNullOrWhiteSpace(s) || s.Trim().Length <= 16)
                .WithMessage("Единица измерения не может быть длиннее 16 символов.")
                .Must(s => string.IsNullOrWhiteSpace(s) || BaseUnitExtensions.TryParseDefaultUnit(s, out _))
                .WithMessage("Единица измерения должна быть одной из: C, %, pH.");
        });

        When(x => x.ValueMin.HasValue && x.ValueMax.HasValue, () =>
        {
            RuleFor(x => x)
                .Must(c => c.ValueMin <= c.ValueMax)
                .WithMessage("Минимальное значение не может быть больше максимального.");
        });
    }
}
