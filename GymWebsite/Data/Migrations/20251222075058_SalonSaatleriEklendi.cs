using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymWebsite.Data.Migrations
{
    /// <inheritdoc />
    public partial class SalonSaatleriEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GymId",
                table: "Trainers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WeekdayHours",
                table: "Gyms",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WeekendHours",
                table: "Gyms",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Trainers_GymId",
                table: "Trainers",
                column: "GymId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trainers_Gyms_GymId",
                table: "Trainers",
                column: "GymId",
                principalTable: "Gyms",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trainers_Gyms_GymId",
                table: "Trainers");

            migrationBuilder.DropIndex(
                name: "IX_Trainers_GymId",
                table: "Trainers");

            migrationBuilder.DropColumn(
                name: "GymId",
                table: "Trainers");

            migrationBuilder.DropColumn(
                name: "WeekdayHours",
                table: "Gyms");

            migrationBuilder.DropColumn(
                name: "WeekendHours",
                table: "Gyms");
        }
    }
}
