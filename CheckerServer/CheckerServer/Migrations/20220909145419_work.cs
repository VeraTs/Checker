using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CheckerServer.Migrations
{
    public partial class work : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DishStatistics");

            migrationBuilder.DropColumn(
                name: "AvrageMonthSales",
                table: "Dishes");

            migrationBuilder.DropColumn(
                name: "LastMonthSales",
                table: "Dishes");

            migrationBuilder.DropColumn(
                name: "ThisMonthSales",
                table: "Dishes");

            migrationBuilder.CreateTable(
                name: "Statistics",
                columns: table => new
                {
                    Month = table.Column<int>(type: "int", nullable: false),
                    DishId = table.Column<int>(type: "int", nullable: false),
                    RestaurantId = table.Column<int>(type: "int", nullable: false),
                    TimesOrdered = table.Column<long>(type: "bigint", nullable: false),
                    AccPrepTime = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statistics", x => new { x.Month, x.DishId });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Statistics");

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

            migrationBuilder.CreateTable(
                name: "DishStatistics",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DishId = table.Column<int>(type: "int", nullable: false),
                    Finish = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Start = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DishStatistics", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DishStatistics_Dishes_DishId",
                        column: x => x.DishId,
                        principalTable: "Dishes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DishStatistics_DishId",
                table: "DishStatistics",
                column: "DishId");
        }
    }
}
