using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccessTokenInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class createaccessdb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccessToken",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OTP",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OtpSubmittedTime",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RequestExpiry",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessToken",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "OTP",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "OtpSubmittedTime",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RequestExpiry",
                table: "Users");
        }
    }
}
