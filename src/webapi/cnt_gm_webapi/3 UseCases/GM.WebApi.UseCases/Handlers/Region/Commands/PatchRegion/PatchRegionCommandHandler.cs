using FluentValidation;

using GM.WebApi.Infrastructure.Interfaces.DataAccess;
using GM.WebApi.UseCases.Exceptions;
using GM.WebApi.UseCases.Handlers.Region.DTOs;
using GM.WebApi.UseCases.Handlers.Region.Mappings;

using Microsoft.EntityFrameworkCore;

using Requestum.Contract;

namespace GM.WebApi.UseCases.Handlers.Region.Commands.PatchRegion;

public sealed class PatchRegionCommandHandler : IAsyncCommandHandler<PatchRegionCommand, RegionDto>
{
    private readonly IDbContext _db;
    private readonly IValidator<PatchRegionCommand> _validator;

    public PatchRegionCommandHandler(IDbContext db, IValidator<PatchRegionCommand> validator)
    {
        _db = db;
        _validator = validator;
    }

    public async Task<RegionDto> ExecuteAsync(PatchRegionCommand command, CancellationToken cancellationToken = default)
    {
        await _validator.ValidateAndThrowAsync(command, cancellationToken);

        var region = await _db.Regions
            .FirstOrDefaultAsync(r => r.Id == command.RegionId, cancellationToken);

        if (region is null)
        {
            throw new UseCaseNotFoundException($"Регион с идентификатором {command.RegionId} не найден.");
        }

        if (command.Code != null)
        {
            var code = command.Code.Trim();
            if (await _db.Regions.AnyAsync(r => r.Code == code && r.Id != command.RegionId, cancellationToken))
            {
                throw new UseCaseConflictException($"Регион с кодом «{code}» уже существует.");
            }

            region.Code = code;
        }

        if (command.Name != null)
        {
            region.Name = command.Name.Trim();
        }

        if (command.IsActive.HasValue)
        {
            region.IsActive = command.IsActive.Value;
        }

        region.UpdatedAt = DateTimeOffset.UtcNow;
        await _db.SaveChangesAsync(cancellationToken);

        return RegionMappings.ToDto(region);
    }
}
