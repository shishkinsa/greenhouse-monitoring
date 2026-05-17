using FluentValidation;

using GM.WebApi.Infrastructure.Interfaces.DataAccess;
using GM.WebApi.UseCases.Exceptions;
using GM.WebApi.UseCases.Extensions;
using GM.WebApi.UseCases.Handlers.SensorTypes.DTOs;
using GM.WebApi.UseCases.Handlers.SensorTypes.Mappings;

using Microsoft.EntityFrameworkCore;

using Requestum.Contract;

using SensorTypeEntity = GM.WebApi.Entities.Models.SensorType;

namespace GM.WebApi.UseCases.Handlers.SensorTypes.Commands.CreateSensorType;

/// <summary>
/// Создание типа датчика в справочнике <c>app.sensor_types</c>.
/// </summary>
public sealed class CreateSensorTypeCommandHandler : IAsyncCommandHandler<CreateSensorTypeCommand, SensorTypeDto>
{
    private readonly IDbContext _db;
    private readonly IValidator<CreateSensorTypeCommand> _validator;

    public CreateSensorTypeCommandHandler(
        IDbContext db,
        IValidator<CreateSensorTypeCommand> validator)
    {
        _db = db;
        _validator = validator;
    }

    public async Task<SensorTypeDto> ExecuteAsync(
        CreateSensorTypeCommand command,
        CancellationToken cancellationToken = default)
    {
        await _validator.ValidateAndThrowAsync(command, cancellationToken);

        if (!SensorCodeExtensions.TryParseSensorCode(command.Code, out var sensorCode))
        {
            throw new InvalidOperationException("Код типа датчика прошёл валидацию, но не распознан.");
        }

        var code = sensorCode.ToCodeString();
        var name = command.Name.Trim();

        if (await _db.SensorTypes.AnyAsync(st => st.Code == code, cancellationToken))
        {
            throw new UseCaseConflictException($"Тип датчика с кодом «{code}» уже существует.");
        }

        string? defaultUnit = null;
        if (!string.IsNullOrWhiteSpace(command.DefaultUnit)
            && BaseUnitExtensions.TryParseDefaultUnit(command.DefaultUnit, out var unit))
        {
            defaultUnit = unit.ToDefaultUnitString();
        }

        var now = DateTimeOffset.UtcNow;
        var entity = new SensorTypeEntity
        {
            Id = Guid.NewGuid(),
            Code = code,
            Name = name,
            DefaultUnit = defaultUnit,
            ValueMin = command.ValueMin,
            ValueMax = command.ValueMax,
            IsActive = true,
            CreatedAt = now,
            UpdatedAt = now,
        };

        _db.SensorTypes.Add(entity);
        await _db.SaveChangesAsync(cancellationToken);

        return SensorTypeMappings.ToDto(entity);
    }
}
