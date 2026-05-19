using FluentValidation;

using GM.WebApi.DataAccess.Postgres.Data;
using GM.WebApi.Entities.Models;
using GM.WebApi.UseCases.Exceptions;
using GM.WebApi.UseCases.Handlers.Location.Commands.PatchLocation;
using GM.WebApi.UseCases.Handlers.Location.Commands.PatchLocation.Validators;

using Microsoft.EntityFrameworkCore;

namespace GM.WebApi.Tests.Locations;

public class PatchLocationCommandHandlerTests
{
    [Fact]
    public async Task ExecuteAsync_updates_name()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(nameof(ExecuteAsync_updates_name))
            .Options;

        var id = Guid.NewGuid();
        var regionId = Guid.NewGuid();
        var created = DateTimeOffset.Parse("2024-01-01T00:00:00Z");

        await using (var arrange = new AppDbContext(options))
        {
            arrange.Regions.Add(new Region
            {
                Id = regionId,
                Code = "r",
                Name = "R",
                IsActive = true,
                CreatedAt = created,
                UpdatedAt = created,
            });
            arrange.Locations.Add(new Location
            {
                Id = id,
                RegionId = regionId,
                Code = "loc",
                Name = "Старое",
                IsActive = true,
                CreatedAt = created,
                UpdatedAt = created,
            });
            await arrange.SaveChangesAsync();
        }

        await using var db = new AppDbContext(options);
        var handler = new PatchLocationCommandHandler(db, new PatchLocationCommandValidator());

        var result = await handler.ExecuteAsync(new PatchLocationCommand
        {
            LocationId = id,
            Name = "  Новое  ",
        });

        Assert.Equal("Новое", result.Name);
    }

    [Fact]
    public async Task ExecuteAsync_code_conflict_in_region_throws_UseCaseConflictException()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(nameof(ExecuteAsync_code_conflict_in_region_throws_UseCaseConflictException))
            .Options;

        var idA = Guid.NewGuid();
        var idB = Guid.NewGuid();
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
            arrange.Locations.AddRange(
                new Location
                {
                    Id = idA,
                    RegionId = regionId,
                    Code = "a",
                    Name = "A",
                    IsActive = true,
                    CreatedAt = now,
                    UpdatedAt = now,
                },
                new Location
                {
                    Id = idB,
                    RegionId = regionId,
                    Code = "b",
                    Name = "B",
                    IsActive = true,
                    CreatedAt = now,
                    UpdatedAt = now,
                });
            await arrange.SaveChangesAsync();
        }

        await using var db = new AppDbContext(options);
        var handler = new PatchLocationCommandHandler(db, new PatchLocationCommandValidator());

        await Assert.ThrowsAsync<UseCaseConflictException>(() =>
            handler.ExecuteAsync(new PatchLocationCommand { LocationId = idA, Code = "b" }));
    }

    [Fact]
    public async Task ExecuteAsync_empty_patch_throws_ValidationException()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(nameof(ExecuteAsync_empty_patch_throws_ValidationException))
            .Options;

        var id = Guid.NewGuid();
        var now = DateTimeOffset.UtcNow;

        await using (var arrange = new AppDbContext(options))
        {
            arrange.Locations.Add(new Location
            {
                Id = id,
                RegionId = Guid.NewGuid(),
                Code = "x",
                Name = "X",
                IsActive = true,
                CreatedAt = now,
                UpdatedAt = now,
            });
            await arrange.SaveChangesAsync();
        }

        await using var db = new AppDbContext(options);
        var handler = new PatchLocationCommandHandler(db, new PatchLocationCommandValidator());

        await Assert.ThrowsAsync<ValidationException>(() =>
            handler.ExecuteAsync(new PatchLocationCommand { LocationId = id }));
    }
}
