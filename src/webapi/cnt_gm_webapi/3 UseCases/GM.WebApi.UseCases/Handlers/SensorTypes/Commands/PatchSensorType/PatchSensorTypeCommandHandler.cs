using FluentValidation;

using GM.WebApi.Infrastructure.Interfaces.DataAccess;
using GM.WebApi.UseCases.Exceptions;
using GM.WebApi.UseCases.Extensions;
using GM.WebApi.UseCases.Handlers.SensorTypes.DTOs;
using GM.WebApi.UseCases.Handlers.SensorTypes.Mappings;

using Microsoft.EntityFrameworkCore;

using Requestum.Contract;

namespace GM.WebApi.UseCases.Handlers.SensorTypes.Commands.PatchSensorType;

/// <summary>
/// Частичное обновление типа датчика в <c>app.sensor_types</c>.
/// </summary>
public sealed class PatchSensorTypeCommandHandler: IAsyncCommandHandler<PatchSensorTypeCommand, SensorTypeDto>
{
    private readonly IDbContext _db;
    private readonly IValidator<PatchSensorTypeCommand> _validator;

    public PatchSensorTypeCommandHandler(
        IDbContext db,
        IValidator<PatchSensorTypeCommand> validator)
    {
        _db = db;
        _validator = validator;
    }

    public async Task<SensorTypeDto> ExecuteAsync(
        PatchSensorTypeCommand command,
        CancellationToken cancellationToken = default)
    {
        await _validator.ValidateAndThrowAsync(command, cancellationToken);

        var entity = await _db.SensorTypes
            .FirstOrDefaultAsync(st => st.Id == command.SensorTypeId, cancellationToken)
            ?? throw new UseCaseNotFoundException(
                $"Тип датчика с идентификатором {command.SensorTypeId} не найден.");

        if(command.Code != null)
        {
            if(!SensorCodeExtensions.TryParseSensorCode(command.Code, out var sensorCode))
            {
                throw new InvalidOperationException("Код типа датчика прошёл валидацию, но не распознан.");
            }

            var code = sensorCode.ToCodeString();
            if(await _db.SensorTypes.AnyAsync(
                    st => st.Code == code && st.Id != command.SensorTypeId,
                    cancellationToken))
            {
                throw new UseCaseConflictException($"Тип датчика с кодом «{code}» уже существует.");
            }

            entity.Code = code;
        }

        if(command.Name != null)
        {
            entity.Name = command.Name.Trim();
        }

        if(command.DefaultUnit != null)
        {
            if(string.IsNullOrWhiteSpace(command.DefaultUnit))
            {
                entity.DefaultUnit = null;
            }
            else if(BaseUnitExtensions.TryParseDefaultUnit(command.DefaultUnit, out var unit))
            {
                entity.DefaultUnit = unit.ToDefaultUnitString();
            }
        }

        if(command.ValueMin.HasValue)
        {
            entity.ValueMin = command.ValueMin;
        }

        if(command.ValueMax.HasValue)
        {
            entity.ValueMax = command.ValueMax;
        }

        if(command.IsActive.HasValue)
        {
            entity.IsActive = command.IsActive.Value;
        }

        if(entity.ValueMin.HasValue && entity.ValueMax.HasValue && entity.ValueMin > entity.ValueMax)
        {
            throw new ValidationException(
            [
                new FluentValidation.Results.ValidationFailure(
                    nameof(PatchSensorTypeCommand.ValueMin),
                    "Минимальное значение не может быть больше максимального.")
            ]);
        }

        entity.UpdatedAt = DateTimeOffset.UtcNow;
        await _db.SaveChangesAsync(cancellationToken);

        return SensorTypeMappings.ToDto(entity);
    }
}
