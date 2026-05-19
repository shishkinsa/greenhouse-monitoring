using FluentValidation;

using GM.WebApi.DataAccess.Postgres.Data;
using GM.WebApi.Entities.Models;
using GM.WebApi.UseCases.Exceptions;
using GM.WebApi.UseCases.Handlers.SensorTypes.Queries.GetSensorTypeById;
using GM.WebApi.UseCases.Handlers.SensorTypes.Queries.GetSensorTypeById.Validators;

using Microsoft.EntityFrameworkCore;

namespace GM.WebApi.Tests.SensorTypes;

public class GetSensorTypeByIdQueryHandlerTests
{
    [Fact]
    public async Task HandleAsync_existing_sensor_type_returns_wrapped_dto()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(nameof(HandleAsync_existing_sensor_type_returns_wrapped_dto))
            .Options;

        var id = Guid.NewGuid();
        var now = DateTimeOffset.Parse("2024-01-02T03:04:05Z");

        await using (var arrange = new AppDbContext(options))
        {
            arrange.SensorTypes.Add(new SensorType
            {
                Id = id,
                Code = "temperature",
                Name = "Температура",
                DefaultUnit = "C",
                ValueMin = -40,
                ValueMax = 60,
                IsActive = true,
                CreatedAt = now,
                UpdatedAt = now,
            });
            await arrange.SaveChangesAsync();
        }

        await using var db = new AppDbContext(options);
        var handler = new GetSensorTypeByIdQueryHandler(db, new GetSensorTypeByIdQueryValidator());

        var result = await handler.HandleAsync(new GetSensorTypeByIdQuery { SensorTypeId = id });

        Assert.NotNull(result.Data);
        Assert.Equal(id, result.Data.Id);
        Assert.Equal("temperature", result.Data.Code);
        Assert.Equal("Температура", result.Data.Name);
        Assert.Equal("C", result.Data.DefaultUnit);
        Assert.Equal(-40, result.Data.ValueMin);
        Assert.Equal(60, result.Data.ValueMax);
        Assert.True(result.Data.IsActive);
        Assert.Equal(now, result.Data.CreatedAt);
        Assert.Equal(now, result.Data.UpdatedAt);
    }

    [Fact]
    public async Task HandleAsync_unknown_id_throws_UseCaseNotFoundException()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(nameof(HandleAsync_unknown_id_throws_UseCaseNotFoundException))
            .Options;

        await using var db = new AppDbContext(options);
        var handler = new GetSensorTypeByIdQueryHandler(db, new GetSensorTypeByIdQueryValidator());

        await Assert.ThrowsAsync<UseCaseNotFoundException>(() =>
            handler.HandleAsync(new GetSensorTypeByIdQuery { SensorTypeId = Guid.NewGuid() }));
    }

    [Fact]
    public async Task HandleAsync_empty_sensor_type_id_throws_ValidationException()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(nameof(HandleAsync_empty_sensor_type_id_throws_ValidationException))
            .Options;

        await using var db = new AppDbContext(options);
        var handler = new GetSensorTypeByIdQueryHandler(db, new GetSensorTypeByIdQueryValidator());

        await Assert.ThrowsAsync<ValidationException>(() =>
            handler.HandleAsync(new GetSensorTypeByIdQuery { SensorTypeId = Guid.Empty }));
    }
}
