using GM.WebApi.DataAccess.Postgres.Data;
using GM.WebApi.Entities.Models;
using GM.WebApi.UseCases.Exceptions;
using GM.WebApi.UseCases.Handlers.SensorTypes.Commands.DeleteSensorType;

using Microsoft.EntityFrameworkCore;

namespace GM.WebApi.Tests.SensorTypes;

public class DeleteSensorTypeCommandHandlerTests
{
    [Fact]
    public async Task ExecuteAsync_active_sensor_type_sets_inactive_and_updates_timestamp()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(nameof(ExecuteAsync_active_sensor_type_sets_inactive_and_updates_timestamp))
            .Options;

        var id = Guid.NewGuid();
        var created = DateTimeOffset.Parse("2024-06-01T12:00:00Z");

        await using(var arrange = new AppDbContext(options))
        {
            arrange.SensorTypes.Add(new SensorType
            {
                Id = id,
                Code = "temperature",
                Name = "Температура",
                IsActive = true,
                CreatedAt = created,
                UpdatedAt = created,
            });
            await arrange.SaveChangesAsync();
        }

        await using var db = new AppDbContext(options);
        var handler = new DeleteSensorTypeCommandHandler(db);

        await handler.ExecuteAsync(new DeleteSensorTypeCommand { SensorTypeId = id });

        var stored = await db.SensorTypes.AsNoTracking().SingleAsync(st => st.Id == id);
        Assert.False(stored.IsActive);
        Assert.Equal(created, stored.CreatedAt);
        Assert.True(stored.UpdatedAt > created);
    }

    [Fact]
    public async Task ExecuteAsync_already_inactive_is_idempotent()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(nameof(ExecuteAsync_already_inactive_is_idempotent))
            .Options;

        var id = Guid.NewGuid();
        var updated = DateTimeOffset.Parse("2024-07-01T00:00:00Z");

        await using(var arrange = new AppDbContext(options))
        {
            arrange.SensorTypes.Add(new SensorType
            {
                Id = id,
                Code = "humidity",
                Name = "Влажность",
                IsActive = false,
                CreatedAt = updated,
                UpdatedAt = updated,
            });
            await arrange.SaveChangesAsync();
        }

        await using var db = new AppDbContext(options);
        var handler = new DeleteSensorTypeCommandHandler(db);

        await handler.ExecuteAsync(new DeleteSensorTypeCommand { SensorTypeId = id });

        var stored = await db.SensorTypes.AsNoTracking().SingleAsync(st => st.Id == id);
        Assert.False(stored.IsActive);
        Assert.Equal(updated, stored.UpdatedAt);
    }

    [Fact]
    public async Task ExecuteAsync_unknown_id_throws_UseCaseNotFoundException()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(nameof(ExecuteAsync_unknown_id_throws_UseCaseNotFoundException))
            .Options;

        await using var db = new AppDbContext(options);
        var handler = new DeleteSensorTypeCommandHandler(db);

        await Assert.ThrowsAsync<UseCaseNotFoundException>(() =>
            handler.ExecuteAsync(new DeleteSensorTypeCommand { SensorTypeId = Guid.NewGuid() }));
    }
}
