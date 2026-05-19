using FluentValidation;

namespace GM.WebApi.UseCases.Handlers.Location.Commands.PatchLocation.Validators;

public sealed class PatchLocationCommandValidator : AbstractValidator<PatchLocationCommand>
{
    public PatchLocationCommandValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.LocationId)
            .NotEqual(Guid.Empty)
            .WithMessage("Идентификатор локации не должен быть пустым.");

        RuleFor(x => x)
            .Must(c => c.RegionId.HasValue || c.Code != null || c.Name != null || c.Address != null
                || c.Latitude.HasValue || c.Longitude.HasValue || c.IsActive.HasValue)
            .WithMessage("Укажите хотя бы одно поле для обновления.");

        When(x => x.RegionId.HasValue, () =>
        {
            RuleFor(x => x.RegionId!.Value)
                .NotEqual(Guid.Empty)
                .WithMessage("Идентификатор региона не должен быть пустым.");
        });

        When(x => x.Code != null, () =>
        {
            RuleFor(x => x.Code!)
                .Must(s => !string.IsNullOrWhiteSpace(s))
                .WithMessage("Код локации не может быть пустым.")
                .Must(s => s.Trim().Length <= 32)
                .WithMessage("Код локации не может быть длиннее 32 символов.");
        });

        When(x => x.Name != null, () =>
        {
            RuleFor(x => x.Name!)
                .Must(s => !string.IsNullOrWhiteSpace(s))
                .WithMessage("Наименование локации не может быть пустым.")
                .Must(s => s.Trim().Length <= 256)
                .WithMessage("Наименование локации не может быть длиннее 256 символов.");
        });

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
