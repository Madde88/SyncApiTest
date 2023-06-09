﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TestGraphQL.Data;

#nullable disable

namespace SyncApiTest.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230329172452_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.4");

            modelBuilder.Entity("TestGraphQL.Models.Dog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int?>("Age")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Breed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Color")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Deleted")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("LastSyncedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("LocalDateUpdate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("OwnerId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ServerDateUpdated")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Dogs");

                    b.HasData(
                        new
                        {
                            Id = new Guid("90b7b98c-505b-48b4-abcc-506d47fbbe61"),
                            Age = 9,
                            Breed = 0,
                            Color = "Black",
                            Deleted = false,
                            Name = "Lucy",
                            OwnerId = new Guid("288bf7c6-163a-4fd4-a8a3-1e8b7ebf089c")
                        },
                        new
                        {
                            Id = new Guid("517a542a-e59f-49f8-b75a-43ebed7e9e79"),
                            Age = 3,
                            Breed = 1,
                            Color = "Golden",
                            Deleted = false,
                            Name = "Ruby",
                            OwnerId = new Guid("288bf7c6-163a-4fd4-a8a3-1e8b7ebf089c")
                        },
                        new
                        {
                            Id = new Guid("416c7350-d994-47ae-aa9d-a9a6eb04bd8a"),
                            Age = 5,
                            Breed = 2,
                            Color = "Brown",
                            Deleted = false,
                            Name = "Max",
                            OwnerId = new Guid("2ebbad77-97e1-4a8c-8684-e0fa3ae6d2df")
                        },
                        new
                        {
                            Id = new Guid("761bc29f-eb54-4cb5-a437-470898c17738"),
                            Age = 2,
                            Breed = 3,
                            Color = "White",
                            Deleted = false,
                            Name = "Buddy",
                            OwnerId = new Guid("2ebbad77-97e1-4a8c-8684-e0fa3ae6d2df")
                        });
                });

            modelBuilder.Entity("TestGraphQL.Models.Owner", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Address")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Age")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Deleted")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("LastSyncedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("LocalDateUpdate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ServerDateUpdated")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Owners");

                    b.HasData(
                        new
                        {
                            Id = new Guid("288bf7c6-163a-4fd4-a8a3-1e8b7ebf089c"),
                            Age = 35,
                            Deleted = false,
                            Name = "John Doe"
                        },
                        new
                        {
                            Id = new Guid("2ebbad77-97e1-4a8c-8684-e0fa3ae6d2df"),
                            Age = 30,
                            Deleted = false,
                            Name = "Jane Doe"
                        });
                });

            modelBuilder.Entity("TestGraphQL.Models.Dog", b =>
                {
                    b.HasOne("TestGraphQL.Models.Owner", "Owner")
                        .WithMany("Dogs")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("TestGraphQL.Models.Owner", b =>
                {
                    b.Navigation("Dogs");
                });
#pragma warning restore 612, 618
        }
    }
}
