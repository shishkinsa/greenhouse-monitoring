using GM.WebApi.Infrastructure.Interfaces.DataAccess;
using GM.WebApi.UseCases.Exceptions;

using Microsoft.EntityFrameworkCore;

using Requestum.Contract;

namespace GM.WebApi.UseCases.Handlers.SensorTypes.Commands.DeleteSensorType;

/// <summary>
/// Деактивация типа датчика в <c>app.sensor_types</c> (идемпотентно, если уже неактивен).
/// </summary>
public sealed class DeleteSensorTypeCommandHandler: IAsyncCommandHandler<DeleteSensorTypeCommand>
{
    private readonly IDbContext _db;

    public DeleteSensorTypeCommandHandler(IDbContext db) => _db = db;

    public async Task ExecuteAsync(
        DeleteSensorTypeCommand command,
        CancellationToken cancellationToken = default)
    {
        var entity = await _db.SensorTypes
            .FirstOrDefaultAsync(st => st.Id == command.SensorTypeId, cancellationToken)
            ?? throw new UseCaseNotFoundException(
                $"Тип датчика с идентификатором {command.SensorTypeId} не найден.");

        if(!entity.IsActive)
        {
            return;
        }

        entity.IsActive = false;
        entity.UpdatedAt = DateTimeOffset.UtcNow;
        await _db.SaveChangesAsync(cancellationToken);
    }
}
