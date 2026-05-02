using GM.WebApi.DataAccess.Postgres.Data;
using GM.WebApi.UseCases.Exceptions;
using GM.WebApi.UseCases.Handlers.Region.Commands.CreateRegion;
using GM.WebApi.UseCases.Handlers.Region.Commands.CreateRegion.Validations;

using FluentValidation;

using Microsoft.EntityFrameworkCore;

namespace GM.WebApi.Tests.Regions;

public class CreateRegionCommandHandlerTests
{
    [Fact]
    public async Task ExecuteAsync_valid_command_persists_and_returns_response()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(nameof(ExecuteAsync_valid_command_persists_and_returns_response))
            .Options;
        var validator = new CreateRegionCommandValidator();
        await using var db = new AppDbContext(options);
        var handler = new CreateRegionCommandHandler(db, validator);

        var result = await handler.ExecuteAsync(new CreateRegionCommand { Code = "r1", Name = "Регион один" });

        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal("r1", result.Code);
        Assert.Equal("Регион один", result.Name);
        Assert.True(result.IsActive);
        Assert.Equal(1, await db.Regions.CountAsync());
    }

    [Fact]
    public async Task ExecuteAsync_invalid_command_throws_ValidationException()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(nameof(ExecuteAsync_invalid_command_throws_ValidationException))
            .Options;
        var validator = new CreateRegionCommandValidator();
        await using var db = new AppDbContext(options);
        var handler = new CreateRegionCommandHandler(db, validator);

        await Assert.ThrowsAsync<ValidationException>(() =>
            handler.ExecuteAsync(new CreateRegionCommand { Code = "   ", Name = "Имя" }));
    }

    [Fact]
    public async Task ExecuteAsync_second_same_code_on_new_context_throws_UseCaseConflictException()
    {
        var dbName = $"create_region_dup_{Guid.NewGuid():N}";
        var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(dbName).Options;
        var validator = new CreateRegionCommandValidator();

        await using (var db = new AppDbContext(options))
        {
            var handler = new CreateRegionCommandHandler(db, validator);
            await handler.ExecuteAsync(new CreateRegionCommand { Code = "dup-code", Name = "Первый" });
        }

        await using var db2 = new AppDbContext(options);
        var handler2 = new CreateRegionCommandHandler(db2, validator);

        await Assert.ThrowsAsync<UseCaseConflictException>(() =>
            handler2.ExecuteAsync(new CreateRegionCommand { Code = "dup-code", Name = "Второй" }));
    }
}
