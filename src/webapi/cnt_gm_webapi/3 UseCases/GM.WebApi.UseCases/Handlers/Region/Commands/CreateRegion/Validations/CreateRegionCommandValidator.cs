using FluentValidation;

namespace GM.WebApi.UseCases.Handlers.Region.Commands.CreateRegion.Validations;

public sealed class CreateRegionCommandValidator : AbstractValidator<CreateRegionCommand>
{
    public CreateRegionCommandValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Code)
            .Must(c => !string.IsNullOrWhiteSpace(c))
            .WithMessage("Код региона обязателен.")
            .Must(c => c.Trim().Length <= 32)
            .WithMessage("Код региона не может быть длиннее 32 символов.");

        RuleFor(x => x.Name)
            .Must(n => !string.IsNullOrWhiteSpace(n))
            .WithMessage("Наименование региона обязательно.")
            .Must(n => n.Trim().Length <= 256)
            .WithMessage("Наименование региона не может быть длиннее 256 символов.");
    }
}
