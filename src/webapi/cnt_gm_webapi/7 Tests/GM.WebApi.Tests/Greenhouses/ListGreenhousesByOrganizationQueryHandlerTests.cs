using GM.WebApi.DataAccess.Postgres.Data;
using GM.WebApi.Entities.Models;
using GM.WebApi.UseCases.Exceptions;
using GM.WebApi.UseCases.Handlers.Greenhouse.Queries.ListGreenhousesByOrganization;
using GM.WebApi.UseCases.Handlers.Greenhouse.Queries.ListGreenhousesByOrganization.Validators;

using FluentValidation;

using Microsoft.EntityFrameworkCore;

namespace GM.WebApi.Tests.Greenhouses;

public class ListGreenhousesByOrganizationQueryHandlerTests
{
    [Fact]
    public async Task HandleAsync_returns_greenhouses_for_organization_sorted_by_code_with_address()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(nameof(HandleAsync_returns_greenhouses_for_organization_sorted_by_code_with_address))
            .Options;

        var orgId = Guid.NewGuid();
        var regionId = Guid.NewGuid();
        var locId = Guid.NewGuid();
        var now = DateTimeOffset.UtcNow;

        await using (var arrange = new AppDbContext(options))
        {
            arrange.Organizations.Add(new Organization
            {
                Id = orgId,
                Code = "org1",
                Name = "Орг",
                IsActive = true,
                CreatedAt = now,
                UpdatedAt = now
            });
            arrange.Regions.Add(new Region
            {
                Id = regionId,
                Code = "r1",
                Name = "Регион",
                IsActive = true,
                CreatedAt = now,
                UpdatedAt = now
            });
            arrange.Locations.Add(new Location
            {
                Id = locId,
                RegionId = regionId,
                Code = "l1",
                Name = "Локация",
                Address = "ул. Тепличная, 1",
                IsActive = true,
                CreatedAt = now,
                UpdatedAt = now
            });
            arrange.Greenhouses.AddRange(
                new Greenhouse
                {
                    Id = Guid.NewGuid(),
                    OrganizationId = orgId,
                    LocationId = locId,
                    Code = "b",
                    Name = "Теплица B",
                    IsActive = true,
                    CreatedAt = now,
                    UpdatedAt = now
                },
                new Greenhouse
                {
                    Id = Guid.NewGuid(),
                    OrganizationId = orgId,
                    LocationId = locId,
                    Code = "a",
                    Name = "Теплица A",
                    IsActive = true,
                    CreatedAt = now,
                    UpdatedAt = now
                });
            await arrange.SaveChangesAsync();
        }

        await using var db = new AppDbContext(options);
        var handler = new ListGreenhousesByOrganizationQueryHandler(
            db,
            new ListGreenhousesByOrganizationQueryValidation());

        var result = await handler.HandleAsync(new ListGreenhousesByOrganizationQuery
        {
            OrganizationId = orgId,
            Page = 1,
            PageSize = 10
        });

        Assert.Equal(2, result.TotalItems);
        Assert.Equal("a", result.Items[0].Code);
        Assert.Equal("b", result.Items[1].Code);
        Assert.Equal(orgId, result.Items[0].OrganisationId);
        Assert.Equal("ул. Тепличная, 1", result.Items[0].Address);
    }

    [Fact]
    public async Task HandleAsync_unknown_organization_throws_UseCaseNotFoundException()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(nameof(HandleAsync_unknown_organization_throws_UseCaseNotFoundException))
            .Options;

        await using var db = new AppDbContext(options);
        var handler = new ListGreenhousesByOrganizationQueryHandler(
            db,
            new ListGreenhousesByOrganizationQueryValidation());

        await Assert.ThrowsAsync<UseCaseNotFoundException>(() =>
            handler.HandleAsync(new ListGreenhousesByOrganizationQuery
            {
                OrganizationId = Guid.NewGuid(),
                Page = 1,
                PageSize = 10
            }));
    }

    [Fact]
    public async Task HandleAsync_empty_organization_id_throws_ValidationException()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(nameof(HandleAsync_empty_organization_id_throws_ValidationException))
            .Options;

        await using var db = new AppDbContext(options);
        var handler = new ListGreenhousesByOrganizationQueryHandler(
            db,
            new ListGreenhousesByOrganizationQueryValidation());

        await Assert.ThrowsAsync<ValidationException>(() =>
            handler.HandleAsync(new ListGreenhousesByOrganizationQuery
            {
                OrganizationId = Guid.Empty,
                Page = 1,
                PageSize = 10
            }));
    }
}
