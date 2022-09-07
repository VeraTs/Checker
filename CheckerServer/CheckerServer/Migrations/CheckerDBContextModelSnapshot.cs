﻿// <auto-generated />
using System;
using CheckerServer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CheckerServer.Migrations
{
    [DbContext(typeof(CheckerDBContext))]
    partial class CheckerDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("CheckerServer.Models.Dish", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<int>("AvrageMonthSales")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("EstMakeTime")
                        .HasColumnType("real");

                    b.Property<int>("LastMonthSales")
                        .HasColumnType("int");

                    b.Property<int>("LineId")
                        .HasColumnType("int");

                    b.Property<int>("MakerFit")
                        .HasColumnType("int");

                    b.Property<int>("MakerId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Price")
                        .HasColumnType("real");

                    b.Property<int>("RestMenuId")
                        .HasColumnType("int");

                    b.Property<int>("ThisMonthSales")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("LineId");

                    b.HasIndex("RestMenuId");

                    b.ToTable("Dishes");
                });

            modelBuilder.Entity("CheckerServer.Models.DishStatistic", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<int>("DishId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Finish")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Start")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.HasIndex("DishId");

                    b.ToTable("DishStatistics");
                });

            modelBuilder.Entity("CheckerServer.Models.Ingredient", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<int>("InStock")
                        .HasColumnType("int");

                    b.Property<string>("MeasurementType")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("MeasurementType");

                    b.ToTable("Ingredients");
                });

            modelBuilder.Entity("CheckerServer.Models.Line", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<int>("Limit")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("RestaurantId")
                        .HasColumnType("int");

                    b.Property<int>("ServingAreaId")
                        .HasColumnType("int");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("RestaurantId");

                    b.HasIndex("ServingAreaId");

                    b.ToTable("Lines");
                });

            modelBuilder.Entity("CheckerServer.Models.Maker", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<int>("LineId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("LineId");

                    b.ToTable("Maker");
                });

            modelBuilder.Entity("CheckerServer.Models.Measurement", b =>
                {
                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Type");

                    b.ToTable("Measurements");
                });

            modelBuilder.Entity("CheckerServer.Models.Order", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<int>("OrderType")
                        .HasColumnType("int");

                    b.Property<float>("RemainsToPay")
                        .HasColumnType("real");

                    b.Property<int>("RestaurantId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("Table")
                        .HasColumnType("int");

                    b.Property<float>("TotalCost")
                        .HasColumnType("real");

                    b.HasKey("ID");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("CheckerServer.Models.OrderItem", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<string>("Changes")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DishId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Finish")
                        .HasColumnType("datetime2");

                    b.Property<int>("LineStatus")
                        .HasColumnType("int");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<int>("ServingAreaZone")
                        .HasColumnType("int");

                    b.Property<DateTime>("Start")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("DishId");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("CheckerServer.Models.Restaurant", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<int>("Balance")
                        .HasColumnType("int");

                    b.Property<string>("ContactName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Restaurants");
                });

            modelBuilder.Entity("CheckerServer.Models.RestMenu", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RestaurantId")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("RestaurantId");

                    b.ToTable("RestMenus");
                });

            modelBuilder.Entity("CheckerServer.Models.ServingArea", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RestaurantId")
                        .HasColumnType("int");

                    b.Property<int>("ZoneNum")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("RestaurantId");

                    b.ToTable("ServingAreas");
                });

            modelBuilder.Entity("CheckerServer.Models.Dish", b =>
                {
                    b.HasOne("CheckerServer.Models.Line", null)
                        .WithMany("Dishes")
                        .HasForeignKey("LineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CheckerServer.Models.RestMenu", null)
                        .WithMany("Dishes")
                        .HasForeignKey("RestMenuId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CheckerServer.Models.DishStatistic", b =>
                {
                    b.HasOne("CheckerServer.Models.Dish", "Dish")
                        .WithMany()
                        .HasForeignKey("DishId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dish");
                });

            modelBuilder.Entity("CheckerServer.Models.Ingredient", b =>
                {
                    b.HasOne("CheckerServer.Models.Measurement", "Measurement")
                        .WithMany()
                        .HasForeignKey("MeasurementType")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Measurement");
                });

            modelBuilder.Entity("CheckerServer.Models.Line", b =>
                {
                    b.HasOne("CheckerServer.Models.Restaurant", null)
                        .WithMany("Lines")
                        .HasForeignKey("RestaurantId");

                    b.HasOne("CheckerServer.Models.ServingArea", "ServingArea")
                        .WithMany()
                        .HasForeignKey("ServingAreaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ServingArea");
                });

            modelBuilder.Entity("CheckerServer.Models.Maker", b =>
                {
                    b.HasOne("CheckerServer.Models.Line", null)
                        .WithMany("Makers")
                        .HasForeignKey("LineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CheckerServer.Models.OrderItem", b =>
                {
                    b.HasOne("CheckerServer.Models.Dish", "Dish")
                        .WithMany()
                        .HasForeignKey("DishId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CheckerServer.Models.Order", null)
                        .WithMany("Items")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dish");
                });

            modelBuilder.Entity("CheckerServer.Models.RestMenu", b =>
                {
                    b.HasOne("CheckerServer.Models.Restaurant", null)
                        .WithMany("Menus")
                        .HasForeignKey("RestaurantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CheckerServer.Models.ServingArea", b =>
                {
                    b.HasOne("CheckerServer.Models.Restaurant", null)
                        .WithMany("ServingAreas")
                        .HasForeignKey("RestaurantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CheckerServer.Models.Line", b =>
                {
                    b.Navigation("Dishes");

                    b.Navigation("Makers");
                });

            modelBuilder.Entity("CheckerServer.Models.Order", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("CheckerServer.Models.Restaurant", b =>
                {
                    b.Navigation("Lines");

                    b.Navigation("Menus");

                    b.Navigation("ServingAreas");
                });

            modelBuilder.Entity("CheckerServer.Models.RestMenu", b =>
                {
                    b.Navigation("Dishes");
                });
#pragma warning restore 612, 618
        }
    }
}
