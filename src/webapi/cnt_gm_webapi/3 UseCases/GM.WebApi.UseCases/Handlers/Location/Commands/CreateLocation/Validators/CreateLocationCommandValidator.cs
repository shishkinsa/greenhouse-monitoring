using FluentValidation;

namespace GM.WebApi.UseCases.Handlers.Location.Commands.CreateLocation.Validators;

public sealed class CreateLocationCommandValidator : AbstractValidator<CreateLocationCommand>
{
    public CreateLocationCommandValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.RegionId)
            .NotEqual(Guid.Empty)
            .WithMessage("Идентификатор региона не должен быть пустым.");

        RuleFor(x => x.Code)
            .Must(c => !string.IsNullOrWhiteSpace(c))
            .WithMessage("Код локации обязателен.")
            .Must(c => c.Trim().Length <= 32)
            .WithMessage("Код локации не может быть длиннее 32 символов.");

        RuleFor(x => x.Name)
            .Must(n => !string.IsNullOrWhiteSpace(n))
            .WithMessage("Наименование локации обязательно.")
            .Must(n => n.Trim().Length <= 256)
            .WithMessage("Наименование локации не может быть длиннее 256 символов.");

        When(x => x.Address != null, () =>
        {
            RuleFor(x => x.Address!)
                .Must(a => a.Trim().Length <= 512)
                .WithMessage("Адрес не может быть длиннее 512 символов.");
        });

        When(x => x.Latitude.HasValue, () =>
        {
            RuleFor(x => x.Latitude!.Value)
                .InclusiveBetween(-90m, 90m)
                .WithMessage("Широта должна быть в диапазоне от -90 до 90.");
        });

        When(x => x.Longitude.HasValue, () =>
        {
            RuleFor(x => x.Longitude!.Value)
                .InclusiveBetween(-180m, 180m)
                .WithMessage("Долгота должна быть в диапазоне от -180 до 180.");
        });
    }
}
