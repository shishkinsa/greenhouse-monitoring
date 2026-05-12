using FluentValidation;

namespace GM.WebApi.UseCases.Handlers.Region.Commands.PatchRegion.Validations;

public sealed class PatchRegionCommandValidator : AbstractValidator<PatchRegionCommand>
{
    public PatchRegionCommandValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.RegionId)
            .NotEqual(Guid.Empty)
            .WithMessage("Идентификатор региона не должен быть пустым.");

        RuleFor(x => x)
            .Must(c => c.Code != null || c.Name != null || c.IsActive.HasValue)
            .WithMessage("Укажите хотя бы одно поле: code, name или isActive.");

        When(x => x.Code != null, () =>
        {
            RuleFor(x => x.Code!)
                .Must(s => !string.IsNullOrWhiteSpace(s))
                .WithMessage("Код региона не может быть пустым.")
                .Must(s => s.Trim().Length <= 32)
                .WithMessage("Код региона не может быть длиннее 32 символов.");
        });

        When(x => x.Name != null, () =>
        {
            RuleFor(x => x.Name!)
                .Must(s => !string.IsNullOrWhiteSpace(s))
                .WithMessage("Наименование региона не может быть пустым.")
                .Must(s => s.Trim().Length <= 256)
                .WithMessage("Наименование региона не может быть длиннее 256 символов.");
        });
    }
}
