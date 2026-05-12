using GM.WebApi.DataAccess.Postgres.Data;
using GM.WebApi.Entities.Models;
using GM.WebApi.UseCases.Exceptions;
using GM.WebApi.UseCases.Handlers.Region.Commands.PatchRegion;
using GM.WebApi.UseCases.Handlers.Region.Commands.PatchRegion.Validations;

using FluentValidation;

using Microsoft.EntityFrameworkCore;

namespace GM.WebApi.Tests.Regions;

public class PatchRegionCommandHandlerTests
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
            arrange.Regions.Add(new Region
            {
                Id = id,
                Code = "c1",
                Name = "Old",
                IsActive = true,
                CreatedAt = created,
                UpdatedAt = created
            });
            await arrange.SaveChangesAsync();
        }

        await using var db = new AppDbContext(options);
        var handler = new PatchRegionCommandHandler(db, new PatchRegionCommandValidator());

        var result = await handler.ExecuteAsync(new PatchRegionCommand
        {
            RegionId = id,
            Name = "  New name  "
        });

        Assert.Equal("c1", result.Code);
        Assert.Equal("New name", result.Name);
        Assert.True(result.IsActive);
        Assert.Equal(created, result.CreatedAt);
        Assert.True(result.UpdatedAt > created);

        var stored = await db.Regions.AsNoTracking().SingleAsync(r => r.Id == id);
        Assert.Equal("New name", stored.Name);
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
            arrange.Regions.Add(new Region
            {
                Id = id,
                Code = "ab",
                Name = "R",
                IsActive = true,
                CreatedAt = now,
                UpdatedAt = now
            });
            await arrange.SaveChangesAsync();
        }

        await using var db = new AppDbContext(options);
        var handler = new PatchRegionCommandHandler(db, new PatchRegionCommandValidator());

        var result = await handler.ExecuteAsync(new PatchRegionCommand { RegionId = id, Code = "  ab  " });

        Assert.Equal("ab", result.Code);
    }

    [Fact]
    public async Task ExecuteAsync_code_taken_by_other_region_throws_UseCaseConflictException()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(nameof(ExecuteAsync_code_taken_by_other_region_throws_UseCaseConflictException))
            .Options;

        var idA = Guid.NewGuid();
        var idB = Guid.NewGuid();
        var now = DateTimeOffset.UtcNow;

        await using (var arrange = new AppDbContext(options))
        {
            arrange.Regions.AddRange(
                new Region { Id = idA, Code = "a", Name = "A", IsActive = true, CreatedAt = now, UpdatedAt = now },
                new Region { Id = idB, Code = "b", Name = "B", IsActive = true, CreatedAt = now, UpdatedAt = now });
            await arrange.SaveChangesAsync();
        }

        await using var db = new AppDbContext(options);
        var handler = new PatchRegionCommandHandler(db, new PatchRegionCommandValidator());

        await Assert.ThrowsAsync<UseCaseConflictException>(() =>
            handler.ExecuteAsync(new PatchRegionCommand { RegionId = idA, Code = "b" }));
    }

    [Fact]
    public async Task ExecuteAsync_unknown_region_throws_UseCaseNotFoundException()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(nameof(ExecuteAsync_unknown_region_throws_UseCaseNotFoundException))
            .Options;

        await using var db = new AppDbContext(options);
        var handler = new PatchRegionCommandHandler(db, new PatchRegionCommandValidator());

        await Assert.ThrowsAsync<UseCaseNotFoundException>(() =>
            handler.ExecuteAsync(new PatchRegionCommand
            {
                RegionId = Guid.NewGuid(),
                Name = "X"
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
            arrange.Regions.Add(new Region
            {
                Id = id,
                Code = "c",
                Name = "N",
                IsActive = true,
                CreatedAt = now,
                UpdatedAt = now
            });
            await arrange.SaveChangesAsync();
        }

        await using var db = new AppDbContext(options);
        var handler = new PatchRegionCommandHandler(db, new PatchRegionCommandValidator());

        await Assert.ThrowsAsync<ValidationException>(() =>
            handler.ExecuteAsync(new PatchRegionCommand { RegionId = id }));
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
            arrange.Regions.Add(new Region
            {
                Id = id,
                Code = "z",
                Name = "Z",
                IsActive = true,
                CreatedAt = now,
                UpdatedAt = now
            });
            await arrange.SaveChangesAsync();
        }

        await using var db = new AppDbContext(options);
        var handler = new PatchRegionCommandHandler(db, new PatchRegionCommandValidator());

        var result = await handler.ExecuteAsync(new PatchRegionCommand { RegionId = id, IsActive = false });

        Assert.False(result.IsActive);
        var stored = await db.Regions.AsNoTracking().SingleAsync(r => r.Id == id);
        Assert.False(stored.IsActive);
    }
}
