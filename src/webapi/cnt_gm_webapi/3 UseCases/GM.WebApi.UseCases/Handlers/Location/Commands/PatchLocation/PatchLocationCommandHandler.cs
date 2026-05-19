using FluentValidation;

using GM.WebApi.Infrastructure.Interfaces.DataAccess;
using GM.WebApi.UseCases.Exceptions;
using GM.WebApi.UseCases.Handlers.Location.DTOs;
using GM.WebApi.UseCases.Handlers.Location.Mappings;

using Microsoft.EntityFrameworkCore;

using Requestum.Contract;

namespace GM.WebApi.UseCases.Handlers.Location.Commands.PatchLocation;

public sealed class PatchLocationCommandHandler : IAsyncCommandHandler<PatchLocationCommand, LocationDto>
{
    private readonly IDbContext _db;
    private readonly IValidator<PatchLocationCommand> _validator;

    public PatchLocationCommandHandler(
        IDbContext db,
        IValidator<PatchLocationCommand> validator)
    {
        _db = db;
        _validator = validator;
    }

    public async Task<LocationDto> ExecuteAsync(
        PatchLocationCommand command,
        CancellationToken cancellationToken = default)
    {
        await _validator.ValidateAndThrowAsync(command, cancellationToken);

        var entity = await _db.Locations
            .FirstOrDefaultAsync(l => l.Id == command.LocationId, cancellationToken)
            ?? throw new UseCaseNotFoundException(
                $"Локация с идентификатором {command.LocationId} не найдена.");

        if (command.RegionId.HasValue)
        {
            if (!await _db.Regions.AnyAsync(r => r.Id == command.RegionId.Value, cancellationToken))
            {
                throw new UseCaseNotFoundException(
                    $"Регион с идентификатором {command.RegionId.Value} не найден.");
            }

            entity.RegionId = command.RegionId.Value;
        }

        if (command.Code != null)
        {
            entity.Code = command.Code.Trim();
        }

        if (command.Name != null)
        {
            entity.Name = command.Name.Trim();
        }

        if (command.Address != null)
        {
            entity.Address = string.IsNullOrWhiteSpace(command.Address)
                ? null
                : command.Address.Trim();
        }

        if (command.Latitude.HasValue)
        {
            entity.Latitude = command.Latitude;
        }

        if (command.Longitude.HasValue)
        {
            entity.Longitude = command.Longitude;
        }

        if (command.IsActive.HasValue)
        {
            entity.IsActive = command.IsActive.Value;
        }

        if (await _db.Locations.AnyAsync(
                l => l.RegionId == entity.RegionId
                    && l.Code == entity.Code
                    && l.Id != command.LocationId,
                cancellationToken))
        {
            throw new UseCaseConflictException(
                $"Локация с кодом «{entity.Code}» уже существует в указанном регионе.");
        }

        entity.UpdatedAt = DateTimeOffset.UtcNow;
        await _db.SaveChangesAsync(cancellationToken);

        return LocationMappings.ToDto(entity);
    }
}
