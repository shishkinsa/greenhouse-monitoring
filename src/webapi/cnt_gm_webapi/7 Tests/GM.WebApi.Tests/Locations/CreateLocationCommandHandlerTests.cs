using FluentValidation;

using GM.WebApi.DataAccess.Postgres.Data;
using GM.WebApi.Entities.Models;
using GM.WebApi.UseCases.Exceptions;
using GM.WebApi.UseCases.Handlers.Location.Commands.CreateLocation;
using GM.WebApi.UseCases.Handlers.Location.Commands.CreateLocation.Validators;

using Microsoft.EntityFrameworkCore;

namespace GM.WebApi.Tests.Locations;

public class CreateLocationCommandHandlerTests
{
    [Fact]
    public async Task ExecuteAsync_valid_command_persists_and_returns_dto()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(nameof(ExecuteAsync_valid_command_persists_and_returns_dto))
            .Options;

        var regionId = Guid.NewGuid();

        await using (var arrange = new AppDbContext(options))
        {
            var now = DateTimeOffset.UtcNow;
            arrange.Regions.Add(new Region
            {
                Id = regionId,
                Code = "r1",
                Name = "Регион",
                IsActive = true,
                CreatedAt = now,
                UpdatedAt = now,
            });
            await arrange.SaveChangesAsync();
        }

        await using var db = new AppDbContext(options);
        var handler = new CreateLocationCommandHandler(db, new CreateLocationCommandValidator());

        var result = await handler.ExecuteAsync(new CreateLocationCommand
        {
            RegionId = regionId,
            Code = "  site-1  ",
            Name = " Площадка ",
            Address = " ул. Тестовая ",
            Latitude = 55.75m,
            Longitude = 37.62m,
        });

        Assert.Equal("site-1", result.Code);
        Assert.Equal("Площадка", result.Name);
        Assert.Equal("ул. Тестовая", result.Address);
        Assert.True(result.IsActive);
        Assert.Equal(1, await db.Locations.CountAsync());
    }

    [Fact]
    public async Task ExecuteAsync_unknown_region_throws_UseCaseNotFoundException()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(nameof(ExecuteAsync_unknown_region_throws_UseCaseNotFoundException))
            .Options;

        await using var db = new AppDbContext(options);
        var handler = new CreateLocationCommandHandler(db, new CreateLocationCommandValidator());

        await Assert.ThrowsAsync<UseCaseNotFoundException>(() =>
            handler.ExecuteAsync(new CreateLocationCommand
            {
                RegionId = Guid.NewGuid(),
                Code = "a",
                Name = "A",
            }));
    }

    [Fact]
    public async Task ExecuteAsync_duplicate_code_in_region_throws_UseCaseConflictException()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(nameof(ExecuteAsync_duplicate_code_in_region_throws_UseCaseConflictException))
            .Options;

        var regionId = Guid.NewGuid();
        var now = DateTimeOffset.UtcNow;

        await using (var arrange = new AppDbContext(options))
        {
            arrange.Regions.Add(new Region
            {
                Id = regionId,
                Code = "r",
                Name = "R",
                IsActive = true,
                CreatedAt = now,
                UpdatedAt = now,
            });
            arrange.Locations.Add(new Location
            {
                Id = Guid.NewGuid(),
                RegionId = regionId,
                Code = "dup",
                Name = "Existing",
                IsActive = true,
                CreatedAt = now,
                UpdatedAt = now,
            });
            await arrange.SaveChangesAsync();
        }

        await using var db = new AppDbContext(options);
        var handler = new CreateLocationCommandHandler(db, new CreateLocationCommandValidator());

        await Assert.ThrowsAsync<UseCaseConflictException>(() =>
            handler.ExecuteAsync(new CreateLocationCommand
            {
                RegionId = regionId,
                Code = "dup",
                Name = "New",
            }));
    }

    [Fact]
    public async Task ExecuteAsync_invalid_latitude_throws_ValidationException()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(nameof(ExecuteAsync_invalid_latitude_throws_ValidationException))
            .Options;

        await using var db = new AppDbContext(options);
        var handler = new CreateLocationCommandHandler(db, new CreateLocationCommandValidator());

        await Assert.ThrowsAsync<ValidationException>(() =>
            handler.ExecuteAsync(new CreateLocationCommand
            {
                RegionId = Guid.NewGuid(),
                Code = "x",
                Name = "X",
                Latitude = 100m,
            }));
    }
}
