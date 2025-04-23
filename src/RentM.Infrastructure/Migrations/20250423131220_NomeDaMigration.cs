using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NomeDaMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DriverLicenseImageUrl",
                table: "DeliveryPersons",
                newName: "DriverLicenseImageBase64");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DriverLicenseImageBase64",
                table: "DeliveryPersons",
                newName: "DriverLicenseImageUrl");
        }
    }
}
