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

                    b.Property<Guid>("AdministrativeAreaLevel2Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ApiPlaceId")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.HasKey("Id");

                    b.HasIndex("AdministrativeAreaLevel2Id");

                    b.HasIndex("ApiPlaceId")
                        .IsUnique();

                    b.ToTable("Address", (string)null);
                });

            modelBuilder.Entity("JourneyMate.Domain.Entities.AdministrativeAreaLevel1", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CountryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LongName")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("ShortName")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.ToTable("AdministrativeAreaLevel1", (string)null);
                });

            modelBuilder.Entity("JourneyMate.Domain.Entities.AdministrativeAreaLevel2", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AdministrativeAreaLevel1Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LongName")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("ShortName")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("AdministrativeAreaLevel1Id");

                    b.ToTable("AdministrativeAreaLevel2", (string)null);
                });

            modelBuilder.Entity("JourneyMate.Domain.Entities.Country", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LongName")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("ShortName")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.ToTable("Country", (string)null);
                });

            modelBuilder.Entity("JourneyMate.Domain.Entities.Follow", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("FollowDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("FollowedId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FollowerId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("FollowedId");

                    b.HasIndex("FollowerId");

                    b.ToTable("Follow", (string)null);
                });

            modelBuilder.Entity("JourneyMate.Domain.Entities.FollowPlanRelation", b =>
                {
                    b.Property<Guid>("FollowId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PlanId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("FollowId", "PlanId");

                    b.HasIndex("PlanId");

                    b.ToTable("FollowPlanRelation", (string)null);
                });

            modelBuilder.Entity("JourneyMate.Domain.Entities.Photo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Data")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<int>("Height")
                        .HasColumnType("int");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhotoReference")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("PlaceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Width")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PlaceId");

                    b.ToTable("Photo", (string)null);
                });

            modelBuilder.Entity("JourneyMate.Domain.Entities.Place", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ApiPlaceId")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<string>("BusinessStatus")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

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
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<double>("Rating")
                        .HasColumnType("float");

                    b.Property<int>("UserRatingsTotal")
                        .HasColumnType("int");

                    b.Property<string>("Vicinity")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.HasKey("Id");

                    b.HasIndex("ApiPlaceId")
                        .IsUnique();

                    b.ToTable("Place", (string)null);
                });

            modelBuilder.Entity("JourneyMate.Domain.Entities.PlaceAddressRelation", b =>
                {
                    b.Property<Guid>("AddressId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PlaceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("DistanceFromAddress")
                        .HasPrecision(8, 2)
                        .HasColumnType("float(8)");

                    b.HasKey("AddressId", "PlaceId");

                    b.HasIndex("PlaceId");

                    b.ToTable("PlaceAddressRelation", (string)null);
                });

            modelBuilder.Entity("JourneyMate.Domain.Entities.PlaceType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ApiName")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("ApiName")
                        .IsUnique();

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("PlaceType", (string)null);
                });

            modelBuilder.Entity("JourneyMate.Domain.Entities.Plan", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

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
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Plan", (string)null);
                });

            modelBuilder.Entity("JourneyMate.Domain.Entities.Report", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AddressId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.HasIndex("UserId");

                    b.ToTable("Report", (string)null);
                });

            modelBuilder.Entity("JourneyMate.Domain.Entities.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Role", (string)null);
                });

            modelBuilder.Entity("JourneyMate.Domain.Entities.Schedule", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("EndingDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("PlaceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PlanId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("StartingDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("PlaceId");

                    b.HasIndex("PlanId");

                    b.ToTable("Schedule", (string)null);
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
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

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
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("PlacePlaceTypeRelation", b =>
                {
                    b.Property<Guid>("PlaceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PlaceTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("PlaceId", "PlaceTypeId");

                    b.HasIndex("PlaceTypeId");

                    b.ToTable("PlacePlaceTypeRelation");
                });

            modelBuilder.Entity("PlacePlanRelation", b =>
                {
                    b.Property<Guid>("PlanId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PlaceId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("PlanId", "PlaceId");

                    b.HasIndex("PlaceId");

                    b.ToTable("PlacePlanRelation");
                });

            modelBuilder.Entity("ReportPlaceRelation", b =>
                {
                    b.Property<Guid>("ReportId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PlaceId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ReportId", "PlaceId");

                    b.HasIndex("PlaceId");

                    b.ToTable("ReportPlaceRelation");
                });

            modelBuilder.Entity("ReportPlaceTypeRelation", b =>
                {
                    b.Property<Guid>("ReportId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PlaceTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ReportId", "PlaceTypeId");

                    b.HasIndex("PlaceTypeId");

                    b.ToTable("ReportPlaceTypeRelation");
                });

            modelBuilder.Entity("UserRoleRelation", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoleRelation");
                });

            modelBuilder.Entity("JourneyMate.Domain.Entities.Address", b =>
                {
                    b.HasOne("JourneyMate.Domain.Entities.AdministrativeAreaLevel2", "AdministrativeAreaLevel2")
                        .WithMany()
                        .HasForeignKey("AdministrativeAreaLevel2Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("JourneyMate.Domain.ValueObjects.Location", "Location", b1 =>
                        {
                            b1.Property<Guid>("AddressId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<double>("Latitude")
                                .HasColumnType("float")
                                .HasColumnName("Latitude");

                            b1.Property<double>("Longitude")
                                .HasColumnType("float")
                                .HasColumnName("Longitude");

                            b1.HasKey("AddressId");

                            b1.ToTable("Address");

                            b1.WithOwner()
                                .HasForeignKey("AddressId");
                        });

                    b.OwnsOne("JourneyMate.Domain.ValueObjects.AddressComponent", "Locality", b1 =>
                        {
                            b1.Property<Guid>("AddressId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("LongName")
                                .IsRequired()
                                .HasMaxLength(256)
                                .HasColumnType("nvarchar(256)")
                                .HasColumnName("LocalityLongName");

                            b1.Property<string>("ShortName")
                                .IsRequired()
                                .HasMaxLength(256)
                                .HasColumnType("nvarchar(256)")
                                .HasColumnName("LocalityShortName");

                            b1.HasKey("AddressId");

                            b1.ToTable("Address");

                            b1.WithOwner()
                                .HasForeignKey("AddressId");
                        });

                    b.Navigation("AdministrativeAreaLevel2");

                    b.Navigation("Locality")
                        .IsRequired();

                    b.Navigation("Location")
                        .IsRequired();
                });

            modelBuilder.Entity("JourneyMate.Domain.Entities.AdministrativeAreaLevel1", b =>
                {
                    b.HasOne("JourneyMate.Domain.Entities.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Country");
                });

            modelBuilder.Entity("JourneyMate.Domain.Entities.AdministrativeAreaLevel2", b =>
                {
                    b.HasOne("JourneyMate.Domain.Entities.AdministrativeAreaLevel1", "AdministrativeAreaLevel1")
                        .WithMany()
                        .HasForeignKey("AdministrativeAreaLevel1Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AdministrativeAreaLevel1");
                });

            modelBuilder.Entity("JourneyMate.Domain.Entities.Follow", b =>
                {
                    b.HasOne("JourneyMate.Domain.Entities.User", "Followed")
                        .WithMany()
                        .HasForeignKey("FollowedId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("JourneyMate.Domain.Entities.User", "Follower")
                        .WithMany("UserFollowers")
                        .HasForeignKey("FollowerId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Followed");

                    b.Navigation("Follower");
                });

            modelBuilder.Entity("JourneyMate.Domain.Entities.FollowPlanRelation", b =>
                {
                    b.HasOne("JourneyMate.Domain.Entities.Follow", "Follow")
                        .WithMany("Shared")
                        .HasForeignKey("FollowId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("JourneyMate.Domain.Entities.Plan", "Plan")
                        .WithMany("Shared")
                        .HasForeignKey("PlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Follow");

                    b.Navigation("Plan");
                });

            modelBuilder.Entity("JourneyMate.Domain.Entities.Photo", b =>
                {
                    b.HasOne("JourneyMate.Domain.Entities.Place", "Place")
                        .WithMany("Photos")
                        .HasForeignKey("PlaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Place");
                });

            modelBuilder.Entity("JourneyMate.Domain.Entities.Place", b =>
                {
                    b.OwnsOne("JourneyMate.Domain.ValueObjects.Location", "Location", b1 =>
                        {
                            b1.Property<Guid>("PlaceId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<double>("Latitude")
                                .HasColumnType("float")
                                .HasColumnName("Latitude");

                            b1.Property<double>("Longitude")
                                .HasColumnType("float")
                                .HasColumnName("Longitude");

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
                                .HasMaxLength(256)
                                .HasColumnType("nvarchar(256)")
                                .HasColumnName("CompoundCode");

                            b1.Property<string>("GlobalCode")
                                .IsRequired()
                                .HasMaxLength(256)
                                .HasColumnType("nvarchar(256)")
                                .HasColumnName("GlobalCode");

                            b1.HasKey("PlaceId");

                            b1.ToTable("Place");

                            b1.WithOwner()
                                .HasForeignKey("PlaceId");
                        });

                    b.Navigation("Location")
                        .IsRequired();

                    b.Navigation("PlusCode")
                        .IsRequired();
                });

            modelBuilder.Entity("JourneyMate.Domain.Entities.PlaceAddressRelation", b =>
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

            modelBuilder.Entity("JourneyMate.Domain.Entities.Plan", b =>
                {
                    b.HasOne("JourneyMate.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("JourneyMate.Domain.Entities.Report", b =>
                {
                    b.HasOne("JourneyMate.Domain.Entities.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("JourneyMate.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");

                    b.Navigation("User");
                });

            modelBuilder.Entity("JourneyMate.Domain.Entities.Schedule", b =>
                {
                    b.HasOne("JourneyMate.Domain.Entities.Place", "Place")
                        .WithMany()
                        .HasForeignKey("PlaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("JourneyMate.Domain.Entities.Plan", "Plan")
                        .WithMany()
                        .HasForeignKey("PlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Place");

                    b.Navigation("Plan");
                });

            modelBuilder.Entity("PlacePlaceTypeRelation", b =>
                {
                    b.HasOne("JourneyMate.Domain.Entities.Place", null)
                        .WithMany()
                        .HasForeignKey("PlaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("JourneyMate.Domain.Entities.PlaceType", null)
                        .WithMany()
                        .HasForeignKey("PlaceTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PlacePlanRelation", b =>
                {
                    b.HasOne("JourneyMate.Domain.Entities.Place", null)
                        .WithMany()
                        .HasForeignKey("PlaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("JourneyMate.Domain.Entities.Plan", null)
                        .WithMany()
                        .HasForeignKey("PlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ReportPlaceRelation", b =>
                {
                    b.HasOne("JourneyMate.Domain.Entities.Place", null)
                        .WithMany()
                        .HasForeignKey("PlaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("JourneyMate.Domain.Entities.Report", null)
                        .WithMany()
                        .HasForeignKey("ReportId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ReportPlaceTypeRelation", b =>
                {
                    b.HasOne("JourneyMate.Domain.Entities.PlaceType", null)
                        .WithMany()
                        .HasForeignKey("PlaceTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("JourneyMate.Domain.Entities.Report", null)
                        .WithMany()
                        .HasForeignKey("ReportId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("UserRoleRelation", b =>
                {
                    b.HasOne("JourneyMate.Domain.Entities.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("JourneyMate.Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("JourneyMate.Domain.Entities.Address", b =>
                {
                    b.Navigation("Places");
                });

            modelBuilder.Entity("JourneyMate.Domain.Entities.Follow", b =>
                {
                    b.Navigation("Shared");
                });

            modelBuilder.Entity("JourneyMate.Domain.Entities.Place", b =>
                {
                    b.Navigation("Addresses");

                    b.Navigation("Photos");
                });

            modelBuilder.Entity("JourneyMate.Domain.Entities.Plan", b =>
                {
                    b.Navigation("Shared");
                });

            modelBuilder.Entity("JourneyMate.Domain.Entities.User", b =>
                {
                    b.Navigation("UserFollowers");
                });
#pragma warning restore 612, 618
        }
    }
}
