using GM.WebApi.Infrastructure.Interfaces.DataAccess;
using GM.WebApi.UseCases.Exceptions;

using Microsoft.EntityFrameworkCore;

using Requestum.Contract;

namespace GM.WebApi.UseCases.Handlers.Location.Commands.DeleteLocation;

public sealed class DeleteLocationCommandHandler : IAsyncCommandHandler<DeleteLocationCommand>
{
    private readonly IDbContext _db;

    public DeleteLocationCommandHandler(IDbContext db) => _db = db;

    public async Task ExecuteAsync(
        DeleteLocationCommand command,
        CancellationToken cancellationToken = default)
    {
        var entity = await _db.Locations
            .FirstOrDefaultAsync(l => l.Id == command.LocationId, cancellationToken)
            ?? throw new UseCaseNotFoundException(
                $"Локация с идентификатором {command.LocationId} не найдена.");

        if (!entity.IsActive)
        {
            return;
        }

        entity.IsActive = false;
        entity.UpdatedAt = DateTimeOffset.UtcNow;
        await _db.SaveChangesAsync(cancellationToken);
    }
}
