using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymWebsite.Data.Migrations
{
    /// <inheritdoc />
    public partial class TrainerGuncellemesi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Specialties_Trainers_TrainerId",
                table: "Specialties");

            migrationBuilder.DropIndex(
                name: "IX_Specialties_TrainerId",
                table: "Specialties");

            migrationBuilder.DropColumn(
                name: "TrainerId",
                table: "Specialties");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Trainers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Trainers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Bio",
                table: "Trainers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldMaxLength: 4000,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Trainers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorkEndHour",
                table: "Trainers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkStartHour",
                table: "Trainers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TrainerId",
                table: "GymServices",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GymServices_TrainerId",
                table: "GymServices",
                column: "TrainerId");

            migrationBuilder.AddForeignKey(
                name: "FK_GymServices_Trainers_TrainerId",
                table: "GymServices",
                column: "TrainerId",
                principalTable: "Trainers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GymServices_Trainers_TrainerId",
                table: "GymServices");

            migrationBuilder.DropIndex(
                name: "IX_GymServices_TrainerId",
                table: "GymServices");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Trainers");

            migrationBuilder.DropColumn(
                name: "WorkEndHour",
                table: "Trainers");

            migrationBuilder.DropColumn(
                name: "WorkStartHour",
                table: "Trainers");

            migrationBuilder.DropColumn(
                name: "TrainerId",
                table: "GymServices");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Trainers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Trainers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Bio",
                table: "Trainers",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TrainerId",
                table: "Specialties",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Specialties_TrainerId",
                table: "Specialties",
                column: "TrainerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Specialties_Trainers_TrainerId",
                table: "Specialties",
                column: "TrainerId",
                principalTable: "Trainers",
                principalColumn: "Id");
        }
    }
}
