using GM.WebApi.DataAccess.Postgres.Data;
using GM.WebApi.Entities.Models;
using GM.WebApi.UseCases.Handlers.Region.Queries.ListRegions;

using Microsoft.EntityFrameworkCore;

namespace GM.WebApi.Tests.Regions;

public class ListRegionsQueryHandlerTests
{
    [Fact]
    public async Task HandleAsync_returns_regions_sorted_by_code_with_paging_metadata()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(nameof(HandleAsync_returns_regions_sorted_by_code_with_paging_metadata))
            .Options;

        var now = DateTimeOffset.UtcNow;

        await using (var arrange = new AppDbContext(options))
        {
            arrange.Regions.AddRange(
                new Region
                {
                    Id = Guid.NewGuid(),
                    Code = "b",
                    Name = "B",
                    IsActive = true,
                    CreatedAt = now,
                    UpdatedAt = now
                },
                new Region
                {
                    Id = Guid.NewGuid(),
                    Code = "a",
                    Name = "A",
                    IsActive = true,
                    CreatedAt = now,
                    UpdatedAt = now
                });
            await arrange.SaveChangesAsync();
        }

        await using var db = new AppDbContext(options);
        var handler = new ListRegionsQueryHandler(db);

        var result = await handler.HandleAsync(new ListRegionsQuery { Page = 1, PageSize = 10 });

        Assert.Equal(2, result.TotalItems);
        Assert.Equal(2, result.Items.Count);
        Assert.Equal("a", result.Items[0].Code);
        Assert.Equal("b", result.Items[1].Code);
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
            arrange.Regions.AddRange(
                new Region { Id = Guid.NewGuid(), Code = "x", Name = "X", IsActive = true, CreatedAt = now, UpdatedAt = now },
                new Region { Id = Guid.NewGuid(), Code = "y", Name = "Y", IsActive = true, CreatedAt = now, UpdatedAt = now });
            await arrange.SaveChangesAsync();
        }

        await using var db = new AppDbContext(options);
        var handler = new ListRegionsQueryHandler(db);

        var result = await handler.HandleAsync(new ListRegionsQuery { Page = 2, PageSize = 1 });

        Assert.Single(result.Items);
        Assert.Equal("y", result.Items[0].Code);
        Assert.Equal(2, result.Page);
        Assert.Equal(1, result.PageSize);
        Assert.Equal(2, result.TotalItems);
    }
}
