using GM.WebApi.DataAccess.Postgres.Data;
using GM.WebApi.Entities.Models;
using GM.WebApi.UseCases.Exceptions;
using GM.WebApi.UseCases.Handlers.Region.Queries.GetRegionById;
using GM.WebApi.UseCases.Handlers.Region.Queries.GetRegionById.Validators;

using FluentValidation;

using Microsoft.EntityFrameworkCore;

namespace GM.WebApi.Tests.Regions;

public class GetRegionByIdQueryHandlerTests
{
    [Fact]
    public async Task HandleAsync_existing_region_returns_wrapped_dto()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(nameof(HandleAsync_existing_region_returns_wrapped_dto))
            .Options;

        var id = Guid.NewGuid();
        var now = DateTimeOffset.Parse("2024-01-02T03:04:05Z");

        await using (var arrange = new AppDbContext(options))
        {
            arrange.Regions.Add(new Region
            {
                Id = id,
                Code = "reg-1",
                Name = "Регион один",
                IsActive = true,
                CreatedAt = now,
                UpdatedAt = now
            });
            await arrange.SaveChangesAsync();
        }

        await using var db = new AppDbContext(options);
        var handler = new GetRegionByIdQueryHandler(db, new GetRegionByIdQueryValidator());

        var result = await handler.HandleAsync(new GetRegionByIdQuery { RegionId = id });

        Assert.NotNull(result.Data);
        Assert.Equal(id, result.Data.Id);
        Assert.Equal("reg-1", result.Data.Code);
        Assert.Equal("Регион один", result.Data.Name);
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
        var handler = new GetRegionByIdQueryHandler(db, new GetRegionByIdQueryValidator());

        await Assert.ThrowsAsync<UseCaseNotFoundException>(() =>
            handler.HandleAsync(new GetRegionByIdQuery { RegionId = Guid.NewGuid() }));
    }

    [Fact]
    public async Task HandleAsync_empty_region_id_throws_ValidationException()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(nameof(HandleAsync_empty_region_id_throws_ValidationException))
            .Options;

        await using var db = new AppDbContext(options);
        var handler = new GetRegionByIdQueryHandler(db, new GetRegionByIdQueryValidator());

        await Assert.ThrowsAsync<ValidationException>(() =>
            handler.HandleAsync(new GetRegionByIdQuery { RegionId = Guid.Empty }));
    }
}
