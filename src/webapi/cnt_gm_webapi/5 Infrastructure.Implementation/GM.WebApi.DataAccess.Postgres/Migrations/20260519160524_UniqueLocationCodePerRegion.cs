using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GM.WebApi.DataAccess.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class UniqueLocationCodePerRegion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "idx_locations_code_region",
                schema: "app",
                table: "locations",
                columns: new[] { "region_id", "code" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "idx_locations_code_region",
                schema: "app",
                table: "locations");
        }
    }
}
