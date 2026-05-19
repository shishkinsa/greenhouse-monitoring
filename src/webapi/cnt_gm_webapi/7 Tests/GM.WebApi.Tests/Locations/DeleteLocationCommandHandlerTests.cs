using GM.WebApi.DataAccess.Postgres.Data;
using GM.WebApi.Entities.Models;
using GM.WebApi.UseCases.Exceptions;
using GM.WebApi.UseCases.Handlers.Location.Commands.DeleteLocation;

using Microsoft.EntityFrameworkCore;

namespace GM.WebApi.Tests.Locations;

public class DeleteLocationCommandHandlerTests
{
    [Fact]
    public async Task ExecuteAsync_active_location_sets_inactive()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(nameof(ExecuteAsync_active_location_sets_inactive))
            .Options;

        var id = Guid.NewGuid();
        var created = DateTimeOffset.Parse("2024-06-01T12:00:00Z");

        await using (var arrange = new AppDbContext(options))
        {
            arrange.Locations.Add(new Location
            {
                Id = id,
                RegionId = Guid.NewGuid(),
                Code = "off",
                Name = "Off",
                IsActive = true,
                CreatedAt = created,
                UpdatedAt = created,
            });
            await arrange.SaveChangesAsync();
        }

        await using var db = new AppDbContext(options);
        var handler = new DeleteLocationCommandHandler(db);

        await handler.ExecuteAsync(new DeleteLocationCommand { LocationId = id });

        var stored = await db.Locations.AsNoTracking().SingleAsync(l => l.Id == id);
        Assert.False(stored.IsActive);
        Assert.True(stored.UpdatedAt > created);
    }

    [Fact]
    public async Task ExecuteAsync_unknown_id_throws_UseCaseNotFoundException()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(nameof(ExecuteAsync_unknown_id_throws_UseCaseNotFoundException))
            .Options;

        await using var db = new AppDbContext(options);
        var handler = new DeleteLocationCommandHandler(db);

        await Assert.ThrowsAsync<UseCaseNotFoundException>(() =>
            handler.ExecuteAsync(new DeleteLocationCommand { LocationId = Guid.NewGuid() }));
    }
}
