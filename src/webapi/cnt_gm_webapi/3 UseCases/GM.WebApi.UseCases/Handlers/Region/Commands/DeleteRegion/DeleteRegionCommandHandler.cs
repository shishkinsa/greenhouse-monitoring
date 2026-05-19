using GM.WebApi.Infrastructure.Interfaces.DataAccess;
using GM.WebApi.UseCases.Exceptions;

using Microsoft.EntityFrameworkCore;

using Requestum.Contract;

namespace GM.WebApi.UseCases.Handlers.Region.Commands.DeleteRegion;

public sealed class DeleteRegionCommandHandler: IAsyncCommandHandler<DeleteRegionCommand>
{
    private readonly IDbContext _db;

    public DeleteRegionCommandHandler(IDbContext db)
    {
        _db = db;
    }

    public async Task ExecuteAsync(DeleteRegionCommand command, CancellationToken cancellationToken = default)
    {
        var region = await _db.Regions
            .FirstOrDefaultAsync(r => r.Id == command.RegionId, cancellationToken) ?? throw new UseCaseNotFoundException($"Регион с идентификатором {command.RegionId} не найден.");

        if(!region.IsActive)
        {
            return;
        }

        region.IsActive = false;
        region.UpdatedAt = DateTimeOffset.UtcNow;
        await _db.SaveChangesAsync(cancellationToken);
    }
}
