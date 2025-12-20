using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymWebsite.Data.Migrations
{
    /// <inheritdoc />
    public partial class second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeactivationDateTime",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Gyms",
                type: "nvarchar(320)",
                maxLength: 320,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Gyms");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeactivationDateTime",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);
        }
    }
}
