using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CheckerServer.Migrations
{
    public partial class UserStatisticMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Restaurants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Restaurants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "Finish",
                table: "OrderItems",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Start",
                table: "OrderItems",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "AvrageMonthSales",
                table: "Dishes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LastMonthSales",
                table: "Dishes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ThisMonthSales",
                table: "Dishes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "Finish",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "Start",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "AvrageMonthSales",
                table: "Dishes");

            migrationBuilder.DropColumn(
                name: "LastMonthSales",
                table: "Dishes");

            migrationBuilder.DropColumn(
                name: "ThisMonthSales",
                table: "Dishes");
        }
    }
}
