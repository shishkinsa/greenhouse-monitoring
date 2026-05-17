using GM.WebApi.DataAccess.Postgres.Data;
using GM.WebApi.Entities.Models;
using GM.WebApi.UseCases.Extensions;
using GM.WebApi.UseCases.Handlers.SensorTypes.Enums;
using GM.WebApi.UseCases.Handlers.SensorTypes.Queries.ListSensorTypes;

using Microsoft.EntityFrameworkCore;

namespace GM.WebApi.Tests.SensorTypes;

public class ListSensorTypesQueryHandlerTests
{
    [Fact]
    public async Task HandleAsync_returns_sensor_types_sorted_by_code_with_paging_metadata()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(nameof(HandleAsync_returns_sensor_types_sorted_by_code_with_paging_metadata))
            .Options;

        var now = DateTimeOffset.UtcNow;

        await using (var arrange = new AppDbContext(options))
        {
            arrange.SensorTypes.AddRange(
                WithTimestamps(CreateSensorType("humidity", "Влажность", BaseUnit.Percent, 0, 100), now),
                WithTimestamps(CreateSensorType("temperature", "Температура", BaseUnit.Celsius, -40, 60), now));
            await arrange.SaveChangesAsync();
        }

        await using var db = new AppDbContext(options);
        var handler = new ListSensorTypesQueryHandler(db);

        var result = await handler.HandleAsync(new ListSensorTypesQuery { Page = 1, PageSize = 10 });

        Assert.Equal(2, result.TotalItems);
        Assert.Equal(2, result.Items.Count);
        Assert.Equal("humidity", result.Items[0].Code);
        Assert.Equal("temperature", result.Items[1].Code);
        Assert.Equal("Влажность", result.Items[0].Name);
        Assert.Equal("%", result.Items[0].DefaultUnit);
        Assert.Equal(1, result.Page);
        Assert.Equal(10, result.PageSize);
    }

    [Fact]
    public async Task HandleAsync_second_page_respects_page_size()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(nameof(HandleAsync_second_page_respects_page_size))
            .Options;

        var now = DateTimeOffset.UtcNow;

        await using (var arrange = new AppDbContext(options))
        {
            arrange.SensorTypes.AddRange(
                WithTimestamps(CreateSensorType("humidity", "Humidity", BaseUnit.Percent, 0, 100), now),
                WithTimestamps(CreateSensorType("temperature", "Temperature", BaseUnit.Celsius, -40, 60), now));
            await arrange.SaveChangesAsync();
        }

        await using var db = new AppDbContext(options);
        var handler = new ListSensorTypesQueryHandler(db);

        var result = await handler.HandleAsync(new ListSensorTypesQuery { Page = 2, PageSize = 1 });

        Assert.Single(result.Items);
        Assert.Equal("temperature", result.Items[0].Code);
        Assert.Equal(2, result.Page);
        Assert.Equal(1, result.PageSize);
        Assert.Equal(2, result.TotalItems);
    }

    [Fact]
    public async Task HandleAsync_empty_database_returns_empty_items_and_zero_total()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(nameof(HandleAsync_empty_database_returns_empty_items_and_zero_total))
            .Options;

        await using var db = new AppDbContext(options);
        var handler = new ListSensorTypesQueryHandler(db);

        var result = await handler.HandleAsync(new ListSensorTypesQuery { Page = 1, PageSize = 10 });

        Assert.Empty(result.Items);
        Assert.Equal(0, result.TotalItems);
        Assert.Equal(1, result.Page);
        Assert.Equal(10, result.PageSize);
    }

    private static SensorType WithTimestamps(SensorType entity, DateTimeOffset timestamp)
    {
        entity.CreatedAt = timestamp;
        entity.UpdatedAt = timestamp;
        return entity;
    }

    private static SensorType CreateSensorType(
        string code,
        string name,
        BaseUnit? defaultUnit,
        decimal? valueMin,
        decimal? valueMax) =>
        new()
        {
            Id = Guid.NewGuid(),
            Code = code,
            Name = name,
            DefaultUnit = defaultUnit?.ToDefaultUnitString(),
            ValueMin = valueMin,
            ValueMax = valueMax,
            IsActive = true,
        };
}
