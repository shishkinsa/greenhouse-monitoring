using FluentValidation;

using GM.WebApi.Infrastructure.Interfaces.DataAccess;
using GM.WebApi.UseCases.Exceptions;
using GM.WebApi.UseCases.Handlers.Region.Commands.CreateRegion.Responses;

using Microsoft.EntityFrameworkCore;

using Requestum.Contract;

using RegionEntity = GM.WebApi.Entities.Models.Region;

namespace GM.WebApi.UseCases.Handlers.Region.Commands.CreateRegion;

public class CreateRegionCommandHandler: IAsyncCommandHandler<CreateRegionCommand, CreateRegionResponse>
{
    private readonly IDbContext _db;
    private readonly IValidator<CreateRegionCommand> _validator;

    public CreateRegionCommandHandler(IDbContext db, IValidator<CreateRegionCommand> validator)
    {
        _db = db;
        _validator = validator;
    }

    public async Task<CreateRegionResponse> ExecuteAsync(CreateRegionCommand command, CancellationToken cancellationToken = default)
    {
        await _validator.ValidateAndThrowAsync(command, cancellationToken);

        command.Code = command.Code.Trim();
        command.Name = command.Name.Trim();

        if(await _db.Regions.AnyAsync(r => r.Code == command.Code, cancellationToken))
        {
            throw new UseCaseConflictException($"Регион с кодом «{command.Code}» уже существует.");
        }

        var now = DateTimeOffset.UtcNow;
        var region = new RegionEntity
        {
            Id = Guid.NewGuid(),
            Code = command.Code,
            Name = command.Name,
            IsActive = true,
            CreatedAt = now,
            UpdatedAt = now
        };

        _db.Regions.Add(region);
        await _db.SaveChangesAsync(cancellationToken);

        return new CreateRegionResponse
        {
            Id = region.Id,
            Code = region.Code,
            Name = region.Name,
            IsActive = region.IsActive,
            CreatedAt = region.CreatedAt,
            UpdatedAt = region.UpdatedAt
        };
    }
}
