using FluentValidation;

namespace GM.WebApi.UseCases.Handlers.Region.Queries.GetRegionById.Validators;

public class GetRegionByIdQueryValidation: AbstractValidator<GetRegionByIdQuery>
{
    public GetRegionByIdQueryValidation()
    {
        RuleFor(x => x.RegionId)
            .NotEqual(Guid.Empty)
            .WithMessage("Идентификатор региона не должен быть пустым.");
    }
}