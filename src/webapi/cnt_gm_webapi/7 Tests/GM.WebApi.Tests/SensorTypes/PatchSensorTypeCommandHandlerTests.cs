using FluentValidation;

using GM.WebApi.DataAccess.Postgres.Data;
using GM.WebApi.Entities.Models;
using GM.WebApi.UseCases.Exceptions;
using GM.WebApi.UseCases.Handlers.SensorTypes.Commands.PatchSensorType;
using GM.WebApi.UseCases.Handlers.SensorTypes.Commands.PatchSensorType.Validators;

using Microsoft.EntityFrameworkCore;

namespace GM.WebApi.Tests.SensorTypes;

public class PatchSensorTypeCommandHandlerTests
{
    [Fact]
    public async Task ExecuteAsync_updates_name_and_returns_dto()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(nameof(ExecuteAsync_updates_name_and_returns_dto))
            .Options;

        var id = Guid.NewGuid();
        var created = DateTimeOffset.Parse("2024-01-01T00:00:00Z");

        await using (var arrange = new AppDbContext(options))
        {
            arrange.SensorTypes.Add(new SensorType
            {
                Id = id,
                Code = "temperature",
                Name = "Старое имя",
                DefaultUnit = "C",
                ValueMin = -40,
                ValueMax = 60,
                IsActive = true,
                CreatedAt = created,
                UpdatedAt = created,
            });
            await arrange.SaveChangesAsync();
        }

        await using var db = new AppDbContext(options);
        var handler = new PatchSensorTypeCommandHandler(db, new PatchSensorTypeCommandValidator());

        var result = await handler.ExecuteAsync(new PatchSensorTypeCommand
        {
            SensorTypeId = id,
            Name = "  Новое имя  ",
        });

        Assert.Equal("temperature", result.Code);
        Assert.Equal("Новое имя", result.Name);
        Assert.Equal("C", result.DefaultUnit);
        Assert.True(result.IsActive);
        Assert.Equal(created, result.CreatedAt);
        Assert.True(result.UpdatedAt > created);

        var stored = await db.SensorTypes.AsNoTracking().SingleAsync(st => st.Id == id);
        Assert.Equal("Новое имя", stored.Name);
    }

    [Fact]
    public async Task ExecuteAsync_same_trimmed_code_as_self_does_not_conflict()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(nameof(ExecuteAsync_same_trimmed_code_as_self_does_not_conflict))
            .Options;

        var id = Guid.NewGuid();
        var now = DateTimeOffset.UtcNow;

        await using (var arrange = new AppDbContext(options))
        {
            arrange.SensorTypes.Add(new SensorType
            {
                Id = id,
                Code = "humidity",
                Name = "Влажность",
                IsActive = true,
                CreatedAt = now,
                UpdatedAt = now,
            });
            await arrange.SaveChangesAsync();
        }

        await using var db = new AppDbContext(options);
        var handler = new PatchSensorTypeCommandHandler(db, new PatchSensorTypeCommandValidator());

        var result = await handler.ExecuteAsync(new PatchSensorTypeCommand
        {
            SensorTypeId = id,
            Code = "  humidity  ",
        });

        Assert.Equal("humidity", result.Code);
    }

    [Fact]
    public async Task ExecuteAsync_code_taken_by_other_type_throws_UseCaseConflictException()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(nameof(ExecuteAsync_code_taken_by_other_type_throws_UseCaseConflictException))
            .Options;

        var idA = Guid.NewGuid();
        var idB = Guid.NewGuid();
        var now = DateTimeOffset.UtcNow;

        await using (var arrange = new AppDbContext(options))
        {
            arrange.SensorTypes.AddRange(
                new SensorType
                {
                    Id = idA,
                    Code = "temperature",
                    Name = "A",
                    IsActive = true,
                    CreatedAt = now,
                    UpdatedAt = now,
                },
                new SensorType
                {
                    Id = idB,
                    Code = "humidity",
                    Name = "B",
                    IsActive = true,
                    CreatedAt = now,
                    UpdatedAt = now,
                });
            await arrange.SaveChangesAsync();
        }

        await using var db = new AppDbContext(options);
        var handler = new PatchSensorTypeCommandHandler(db, new PatchSensorTypeCommandValidator());

        await Assert.ThrowsAsync<UseCaseConflictException>(() =>
            handler.ExecuteAsync(new PatchSensorTypeCommand { SensorTypeId = idA, Code = "humidity" }));
    }

    [Fact]
    public async Task ExecuteAsync_unknown_id_throws_UseCaseNotFoundException()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(nameof(ExecuteAsync_unknown_id_throws_UseCaseNotFoundException))
            .Options;

        await using var db = new AppDbContext(options);
        var handler = new PatchSensorTypeCommandHandler(db, new PatchSensorTypeCommandValidator());

        await Assert.ThrowsAsync<UseCaseNotFoundException>(() =>
            handler.ExecuteAsync(new PatchSensorTypeCommand
            {
                SensorTypeId = Guid.NewGuid(),
                Name = "X",
            }));
    }

    [Fact]
    public async Task ExecuteAsync_no_patch_fields_throws_ValidationException()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(nameof(ExecuteAsync_no_patch_fields_throws_ValidationException))
            .Options;

        var id = Guid.NewGuid();
        var now = DateTimeOffset.UtcNow;

        await using (var arrange = new AppDbContext(options))
        {
            arrange.SensorTypes.Add(new SensorType
            {
                Id = id,
                Code = "temperature",
                Name = "T",
                IsActive = true,
                CreatedAt = now,
                UpdatedAt = now,
            });
            await arrange.SaveChangesAsync();
        }

        await using var db = new AppDbContext(options);
        var handler = new PatchSensorTypeCommandHandler(db, new PatchSensorTypeCommandValidator());

        await Assert.ThrowsAsync<ValidationException>(() =>
            handler.ExecuteAsync(new PatchSensorTypeCommand { SensorTypeId = id }));
    }

    [Fact]
    public async Task ExecuteAsync_isActive_false_persists()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(nameof(ExecuteAsync_isActive_false_persists))
            .Options;

        var id = Guid.NewGuid();
        var now = DateTimeOffset.UtcNow;

        await using (var arrange = new AppDbContext(options))
        {
            arrange.SensorTypes.Add(new SensorType
            {
                Id = id,
                Code = "soil_ph",
                Name = "pH",
                IsActive = true,
                CreatedAt = now,
                UpdatedAt = now,
            });
            await arrange.SaveChangesAsync();
        }

        await using var db = new AppDbContext(options);
        var handler = new PatchSensorTypeCommandHandler(db, new PatchSensorTypeCommandValidator());

        var result = await handler.ExecuteAsync(new PatchSensorTypeCommand
        {
            SensorTypeId = id,
            IsActive = false,
        });

        Assert.False(result.IsActive);
        var stored = await db.SensorTypes.AsNoTracking().SingleAsync(st => st.Id == id);
        Assert.False(stored.IsActive);
    }
}
