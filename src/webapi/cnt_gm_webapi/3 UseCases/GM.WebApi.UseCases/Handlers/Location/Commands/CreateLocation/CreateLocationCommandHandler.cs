using FluentValidation;

using GM.WebApi.Infrastructure.Interfaces.DataAccess;
using GM.WebApi.UseCases.Exceptions;
using GM.WebApi.UseCases.Handlers.Location.DTOs;
using GM.WebApi.UseCases.Handlers.Location.Mappings;

using Microsoft.EntityFrameworkCore;

using Requestum.Contract;

using LocationEntity = GM.WebApi.Entities.Models.Location;

namespace GM.WebApi.UseCases.Handlers.Location.Commands.CreateLocation;

public sealed class CreateLocationCommandHandler : IAsyncCommandHandler<CreateLocationCommand, LocationDto>
{
    private readonly IDbContext _db;
    private readonly IValidator<CreateLocationCommand> _validator;

    public CreateLocationCommandHandler(
        IDbContext db,
        IValidator<CreateLocationCommand> validator)
    {
        _db = db;
        _validator = validator;
    }

    public async Task<LocationDto> ExecuteAsync(
        CreateLocationCommand command,
        CancellationToken cancellationToken = default)
    {
        await _validator.ValidateAndThrowAsync(command, cancellationToken);

        if (!await _db.Regions.AnyAsync(r => r.Id == command.RegionId, cancellationToken))
        {
            throw new UseCaseNotFoundException(
                $"Регион с идентификатором {command.RegionId} не найден.");
        }

        var code = command.Code.Trim();
        var name = command.Name.Trim();

        if (await _db.Locations.AnyAsync(
                l => l.RegionId == command.RegionId && l.Code == code,
                cancellationToken))
        {
            throw new UseCaseConflictException(
                $"Локация с кодом «{code}» уже существует в указанном регионе.");
        }

        var now = DateTimeOffset.UtcNow;
        var entity = new LocationEntity
        {
            Id = Guid.NewGuid(),
            RegionId = command.RegionId,
            Code = code,
            Name = name,
            Address = NormalizeOptionalText(command.Address),
            Latitude = command.Latitude,
            Longitude = command.Longitude,
            IsActive = true,
            CreatedAt = now,
            UpdatedAt = now,
        };

        _db.Locations.Add(entity);
        await _db.SaveChangesAsync(cancellationToken);

        return LocationMappings.ToDto(entity);
    }

    private static string? NormalizeOptionalText(string? value) =>
        value is null ? null : string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
