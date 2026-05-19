using FluentValidation;

using GM.WebApi.DataAccess.Postgres.Data;
using GM.WebApi.UseCases.Exceptions;
using GM.WebApi.UseCases.Handlers.SensorTypes.Commands.CreateSensorType;
using GM.WebApi.UseCases.Handlers.SensorTypes.Commands.CreateSensorType.Validators;
using GM.WebApi.UseCases.Handlers.SensorTypes.Enums;

using Microsoft.EntityFrameworkCore;

namespace GM.WebApi.Tests.SensorTypes;

public class CreateSensorTypeCommandHandlerTests
{
    [Fact]
    public async Task ExecuteAsync_valid_command_persists_and_returns_dto()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(nameof(ExecuteAsync_valid_command_persists_and_returns_dto))
            .Options;
        var validator = new CreateSensorTypeCommandValidator();
        await using var db = new AppDbContext(options);
        var handler = new CreateSensorTypeCommandHandler(db, validator);

        var result = await handler.ExecuteAsync(new CreateSensorTypeCommand
        {
            Code = "temperature",
            Name = "Температура",
            DefaultUnit = "C",
            ValueMin = -40,
            ValueMax = 60,
        });

        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal("temperature", result.Code);
        Assert.Equal("Температура", result.Name);
        Assert.Equal("C", result.DefaultUnit);
        Assert.Equal(-40, result.ValueMin);
        Assert.Equal(60, result.ValueMax);
        Assert.True(result.IsActive);
        Assert.Equal(1, await db.SensorTypes.CountAsync());
    }

    [Fact]
    public async Task ExecuteAsync_invalid_code_throws_ValidationException()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(nameof(ExecuteAsync_invalid_code_throws_ValidationException))
            .Options;
        var validator = new CreateSensorTypeCommandValidator();
        await using var db = new AppDbContext(options);
        var handler = new CreateSensorTypeCommandHandler(db, validator);

        await Assert.ThrowsAsync<ValidationException>(() =>
            handler.ExecuteAsync(new CreateSensorTypeCommand
            {
                Code = "unknown_type",
                Name = "Тест",
            }));
    }

    [Fact]
    public async Task ExecuteAsync_value_min_greater_than_max_throws_ValidationException()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(nameof(ExecuteAsync_value_min_greater_than_max_throws_ValidationException))
            .Options;
        var validator = new CreateSensorTypeCommandValidator();
        await using var db = new AppDbContext(options);
        var handler = new CreateSensorTypeCommandHandler(db, validator);

        await Assert.ThrowsAsync<ValidationException>(() =>
            handler.ExecuteAsync(new CreateSensorTypeCommand
            {
                Code = "humidity",
                Name = "Влажность",
                ValueMin = 100,
                ValueMax = 0,
            }));
    }

    [Fact]
    public async Task ExecuteAsync_second_same_code_throws_UseCaseConflictException()
    {
        var dbName = $"create_sensor_type_dup_{Guid.NewGuid():N}";
        var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(dbName).Options;
        var validator = new CreateSensorTypeCommandValidator();

        await using (var db = new AppDbContext(options))
        {
            var handler = new CreateSensorTypeCommandHandler(db, validator);
            await handler.ExecuteAsync(new CreateSensorTypeCommand
            {
                Code = "humidity",
                Name = "Первый",
            });
        }

        await using var db2 = new AppDbContext(options);
        var handler2 = new CreateSensorTypeCommandHandler(db2, validator);

        await Assert.ThrowsAsync<UseCaseConflictException>(() =>
            handler2.ExecuteAsync(new CreateSensorTypeCommand
            {
                Code = "HUMIDITY",
                Name = "Второй",
            }));
    }
}
