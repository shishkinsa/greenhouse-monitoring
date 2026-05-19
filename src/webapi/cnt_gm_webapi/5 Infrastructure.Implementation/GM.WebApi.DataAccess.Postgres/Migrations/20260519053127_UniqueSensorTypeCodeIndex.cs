using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GM.WebApi.DataAccess.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class UniqueSensorTypeCodeIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "idx_sensor_types_code",
                schema: "app",
                table: "sensor_types",
                column: "code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "idx_sensor_types_code",
                schema: "app",
                table: "sensor_types");
        }
    }
}
