﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApplication2.Database;

#nullable disable

namespace WebApplication2.Migrations
{
    [DbContext(typeof(WebAppDBContext))]
    [Migration("20230804184524_Inital")]
    partial class Inital
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WebApplication2.Models.Brand", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Brands");
                });

            modelBuilder.Entity("WebApplication2.Models.Laptop", b =>
                {
                    b.Property<Guid>("Number")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BrandId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Condition")
                        .HasColumnType("int");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Number");

                    b.HasIndex("BrandId");

                    b.ToTable("Laptops");
                });

            modelBuilder.Entity("WebApplication2.Models.Store", b =>
                {
                    b.Property<Guid>("StoreNumber")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Province")
                        .HasColumnType("int");

                    b.HasKey("StoreNumber");

                    b.ToTable("Stores");
                });

            modelBuilder.Entity("WebApplication2.Models.StoreLaptopStock", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("LaptopId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<Guid>("StoreId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("LaptopId");

                    b.HasIndex("StoreId");

                    b.ToTable("StoreLaptopStocks");
                });

            modelBuilder.Entity("WebApplication2.Models.Laptop", b =>
                {
                    b.HasOne("WebApplication2.Models.Brand", "Brand")
                        .WithMany()
                        .HasForeignKey("BrandId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Brand");
                });

            modelBuilder.Entity("WebApplication2.Models.StoreLaptopStock", b =>
                {
                    b.HasOne("WebApplication2.Models.Laptop", "Laptop")
                        .WithMany("StoreLaptopStocks")
                        .HasForeignKey("LaptopId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApplication2.Models.Store", "Store")
                        .WithMany("StoreLaptopStocks")
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Laptop");

                    b.Navigation("Store");
                });

            modelBuilder.Entity("WebApplication2.Models.Laptop", b =>
                {
                    b.Navigation("StoreLaptopStocks");
                });

            modelBuilder.Entity("WebApplication2.Models.Store", b =>
                {
                    b.Navigation("StoreLaptopStocks");
                });
#pragma warning restore 612, 618
        }
    }
}
