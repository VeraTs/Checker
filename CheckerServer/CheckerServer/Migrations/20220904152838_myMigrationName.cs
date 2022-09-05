using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CheckerServer.Migrations
{
    public partial class myMigrationName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "Finish",
                table: "OrderItems",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Start",
                table: "OrderItems",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

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
