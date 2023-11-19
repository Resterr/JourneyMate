﻿// <auto-generated />
using System;
using JourneyMate.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace JourneyMate.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("JourneyMate.Domain.Entities.Address", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AdministrativeAreaLevel1")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AdministrativeAreaLevel2")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ApiPlaceId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Locality")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ApiPlaceId")
                        .IsUnique();

                    b.ToTable("Address", (string)null);
                });

            modelBuilder.Entity("JourneyMate.Domain.Entities.Place", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ApiPlaceId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("BusinessStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Rating")
                        .HasColumnType("float");

                    b.Property<int>("UserRatingsTotal")
                        .HasColumnType("int");

                    b.Property<string>("Vicinity")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ApiPlaceId")
                        .IsUnique();

                    b.ToTable("Place", (string)null);
                });

            modelBuilder.Entity("JourneyMate.Domain.Entities.PlaceAddress", b =>
                {
                    b.Property<Guid>("AddressId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PlaceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("DistanceFromAddress")
                        .HasColumnType("float");

                    b.HasKey("AddressId", "PlaceId");

                    b.HasIndex("PlaceId");

                    b.ToTable("PlaceAddress", (string)null);
                });

            modelBuilder.Entity("JourneyMate.Domain.Entities.PlaceType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("PlaceType", (string)null);
                });

            modelBuilder.Entity("JourneyMate.Domain.Entities.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Role", (string)null);
                });

            modelBuilder.Entity("JourneyMate.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("RefreshTokenExpiryTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("PlacePlaceType", b =>
                {
                    b.Property<Guid>("PlacesId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TypesId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("PlacesId", "TypesId");

                    b.HasIndex("TypesId");

                    b.ToTable("PlaceTypeRelation", (string)null);
                });

            modelBuilder.Entity("RoleUser", b =>
                {
                    b.Property<Guid>("RolesId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UsersId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("RolesId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("UserRole", (string)null);
                });

            modelBuilder.Entity("JourneyMate.Domain.Entities.Address", b =>
                {
                    b.OwnsOne("JourneyMate.Domain.ValueObjects.Location", "Location", b1 =>
                        {
                            b1.Property<Guid>("AddressId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<double>("Latitude")
                                .HasColumnType("float");

                            b1.Property<double>("Longitude")
                                .HasColumnType("float");

                            b1.HasKey("AddressId");

                            b1.ToTable("Address");

                            b1.WithOwner()
                                .HasForeignKey("AddressId");
                        });

                    b.Navigation("Location")
                        .IsRequired();
                });

            modelBuilder.Entity("JourneyMate.Domain.Entities.Place", b =>
                {
                    b.OwnsOne("JourneyMate.Domain.ValueObjects.Location", "Location", b1 =>
                        {
                            b1.Property<Guid>("PlaceId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<double>("Latitude")
                                .HasColumnType("float");

                            b1.Property<double>("Longitude")
                                .HasColumnType("float");

                            b1.HasKey("PlaceId");

                            b1.ToTable("Place");

                            b1.WithOwner()
                                .HasForeignKey("PlaceId");
                        });

                    b.OwnsOne("JourneyMate.Domain.ValueObjects.Photo", "Photo", b1 =>
                        {
                            b1.Property<Guid>("PlaceId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<int?>("Height")
                                .IsRequired()
                                .HasColumnType("int");

                            b1.Property<string>("PhotoReference")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<int?>("Width")
                                .IsRequired()
                                .HasColumnType("int");

                            b1.HasKey("PlaceId");

                            b1.ToTable("Place");

                            b1.WithOwner()
                                .HasForeignKey("PlaceId");
                        });

                    b.OwnsOne("JourneyMate.Domain.ValueObjects.PlusCode", "PlusCode", b1 =>
                        {
                            b1.Property<Guid>("PlaceId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("CompoundCode")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("GlobalCode")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("PlaceId");

                            b1.ToTable("Place");

                            b1.WithOwner()
                                .HasForeignKey("PlaceId");
                        });

                    b.Navigation("Location")
                        .IsRequired();

                    b.Navigation("Photo");

                    b.Navigation("PlusCode")
                        .IsRequired();
                });

            modelBuilder.Entity("JourneyMate.Domain.Entities.PlaceAddress", b =>
                {
                    b.HasOne("JourneyMate.Domain.Entities.Address", "Address")
                        .WithMany("Places")
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("JourneyMate.Domain.Entities.Place", "Place")
                        .WithMany("Addresses")
                        .HasForeignKey("PlaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");

                    b.Navigation("Place");
                });

            modelBuilder.Entity("PlacePlaceType", b =>
                {
                    b.HasOne("JourneyMate.Domain.Entities.Place", null)
                        .WithMany()
                        .HasForeignKey("PlacesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("JourneyMate.Domain.Entities.PlaceType", null)
                        .WithMany()
                        .HasForeignKey("TypesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RoleUser", b =>
                {
                    b.HasOne("JourneyMate.Domain.Entities.Role", null)
                        .WithMany()
                        .HasForeignKey("RolesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("JourneyMate.Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("JourneyMate.Domain.Entities.Address", b =>
                {
                    b.Navigation("Places");
                });

            modelBuilder.Entity("JourneyMate.Domain.Entities.Place", b =>
                {
                    b.Navigation("Addresses");
                });
#pragma warning restore 612, 618
        }
    }
}
