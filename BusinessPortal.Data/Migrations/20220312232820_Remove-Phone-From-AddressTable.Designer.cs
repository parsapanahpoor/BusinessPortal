﻿// <auto-generated />
using System;
using BusinessPortal.Data.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BusinessPortal.Data.Migrations
{
    [DbContext(typeof(BusinessPortalDbContext))]
    [Migration("20220312232820_Remove-Phone-From-AddressTable")]
    partial class RemovePhoneFromAddressTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("BusinessPortal.Domain.Entities.Account.Role", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(20,0)");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<decimal>("Id"), 1L, 1);

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<string>("RoleUniqueName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("BusinessPortal.Domain.Entities.Account.RolePermission", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(20,0)");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<decimal>("Id"), 1L, 1);

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<decimal>("PermissionId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<decimal>("RoleId")
                        .HasColumnType("decimal(20,0)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("RolePermissions");
                });

            modelBuilder.Entity("BusinessPortal.Domain.Entities.Account.User", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(20,0)");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<decimal>("Id"), 1L, 1);

                    b.Property<string>("Avatar")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("BanForComment")
                        .HasColumnType("bit");

                    b.Property<bool>("BanForTicket")
                        .HasColumnType("bit");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("EmailActivationCode")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("bit");

                    b.Property<bool>("IsBan")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<bool>("IsEmailConfirm")
                        .HasColumnType("bit");

                    b.Property<bool>("IsMobileConfirm")
                        .HasColumnType("bit");

                    b.Property<string>("Mobile")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("MobileActivationCode")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BusinessPortal.Domain.Entities.Account.UserRole", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(20,0)");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<decimal>("Id"), 1L, 1);

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<decimal>("RoleId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<decimal>("UserId")
                        .HasColumnType("decimal(20,0)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("BusinessPortal.Domain.Entities.Address.Address", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(20,0)");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<decimal>("Id"), 1L, 1);

                    b.Property<string>("AddressTitle")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<decimal?>("CityId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<decimal>("CountryId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<string>("Mobile")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<decimal>("StateId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<string>("UserAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("UserId")
                        .HasColumnType("decimal(20,0)");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.HasIndex("CountryId");

                    b.HasIndex("StateId");

                    b.HasIndex("UserId");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("BusinessPortal.Domain.Entities.Advertisement.Advertisement", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(20,0)");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<decimal>("Id"), 1L, 1);

                    b.Property<decimal?>("AddressId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<decimal?>("AddressId1")
                        .HasColumnType("decimal(20,0)");

                    b.Property<string>("AdsUrl")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("AdvertisementStatus")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("FromCustomer")
                        .HasColumnType("bit");

                    b.Property<bool>("FromEmployee")
                        .HasColumnType("bit");

                    b.Property<string>("ImageName")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<string>("RejectDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(400)
                        .HasColumnType("nvarchar(400)");

                    b.Property<decimal>("UserId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<int>("VisitCount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.HasIndex("AddressId1");

                    b.HasIndex("UserId");

                    b.ToTable("Advertisement");
                });

            modelBuilder.Entity("BusinessPortal.Domain.Entities.Advertisement.AdvertisementSelectedCategory", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(20,0)");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<decimal>("Id"), 1L, 1);

                    b.Property<decimal>("AdsCategoryID")
                        .HasColumnType("decimal(20,0)");

                    b.Property<decimal>("AdvertisementID")
                        .HasColumnType("decimal(20,0)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("AdsCategoryID");

                    b.HasIndex("AdvertisementID");

                    b.ToTable("AdvertisementSelectedCategories");
                });

            modelBuilder.Entity("BusinessPortal.Domain.Entities.Advertisement.AdvertisementTag", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(20,0)");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<decimal>("Id"), 1L, 1);

                    b.Property<decimal>("AdvertisementId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<string>("TagTitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AdvertisementId");

                    b.ToTable("AdvertisementTags");
                });

            modelBuilder.Entity("BusinessPortal.Domain.Entities.BrowseCategory.Category", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(20,0)");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<decimal>("Id"), 1L, 1);

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<decimal?>("ParentId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<int>("Priority")
                        .HasColumnType("int");

                    b.Property<string>("UrlName")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("BusinessPortal.Domain.Entities.Location.State", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(20,0)");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<decimal>("Id"), 1L, 1);

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<decimal?>("ParentId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<string>("UniqueName")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("States");
                });

            modelBuilder.Entity("BusinessPortal.Domain.Entities.SiteSetting.EmailSetting", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(20,0)");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<decimal>("Id"), 1L, 1);

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<bool>("EnableSsL")
                        .HasColumnType("bit");

                    b.Property<string>("From")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<bool>("IsDefaultEmail")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("Port")
                        .HasColumnType("int");

                    b.Property<string>("Smtp")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.ToTable("EmailSettings");

                    b.HasData(
                        new
                        {
                            Id = 1m,
                            CreateDate = new DateTime(2022, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DisplayName = "BusinessPortal",
                            EnableSsL = true,
                            From = "parsapanahpoor77@gmail.com",
                            IsDefaultEmail = true,
                            IsDelete = false,
                            Password = "54511441",
                            Port = 587,
                            Smtp = "smtp.gmail.com",
                            UserName = "BusinessPortal"
                        });
                });

            modelBuilder.Entity("BusinessPortal.Domain.Entities.Account.RolePermission", b =>
                {
                    b.HasOne("BusinessPortal.Domain.Entities.Account.Role", "Role")
                        .WithMany("RolePermissions")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("BusinessPortal.Domain.Entities.Account.UserRole", b =>
                {
                    b.HasOne("BusinessPortal.Domain.Entities.Account.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("BusinessPortal.Domain.Entities.Account.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BusinessPortal.Domain.Entities.Address.Address", b =>
                {
                    b.HasOne("BusinessPortal.Domain.Entities.Location.State", "LocationCity")
                        .WithMany("AddressesCity")
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("BusinessPortal.Domain.Entities.Location.State", "LocationCountry")
                        .WithMany("AddressesCountry")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("BusinessPortal.Domain.Entities.Location.State", "LocationState")
                        .WithMany("AddressesState")
                        .HasForeignKey("StateId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("BusinessPortal.Domain.Entities.Account.User", "User")
                        .WithMany("Addresses")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("LocationCity");

                    b.Navigation("LocationCountry");

                    b.Navigation("LocationState");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BusinessPortal.Domain.Entities.Advertisement.Advertisement", b =>
                {
                    b.HasOne("BusinessPortal.Domain.Entities.Location.State", "State")
                        .WithMany()
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("BusinessPortal.Domain.Entities.Address.Address", null)
                        .WithMany("Advertisement")
                        .HasForeignKey("AddressId1")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("BusinessPortal.Domain.Entities.Account.User", "User")
                        .WithMany("Advertisements")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("State");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BusinessPortal.Domain.Entities.Advertisement.AdvertisementSelectedCategory", b =>
                {
                    b.HasOne("BusinessPortal.Domain.Entities.BrowseCategory.Category", "AdvertisementCategory")
                        .WithMany("AdvertisementSelectedCategory")
                        .HasForeignKey("AdsCategoryID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("BusinessPortal.Domain.Entities.Advertisement.Advertisement", "Advertisement")
                        .WithMany("AdvertisementSelectedCategory")
                        .HasForeignKey("AdvertisementID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Advertisement");

                    b.Navigation("AdvertisementCategory");
                });

            modelBuilder.Entity("BusinessPortal.Domain.Entities.Advertisement.AdvertisementTag", b =>
                {
                    b.HasOne("BusinessPortal.Domain.Entities.Advertisement.Advertisement", "Advertisement")
                        .WithMany("AdvertisementTags")
                        .HasForeignKey("AdvertisementId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Advertisement");
                });

            modelBuilder.Entity("BusinessPortal.Domain.Entities.BrowseCategory.Category", b =>
                {
                    b.HasOne("BusinessPortal.Domain.Entities.BrowseCategory.Category", null)
                        .WithMany("Categories")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("BusinessPortal.Domain.Entities.Location.State", b =>
                {
                    b.HasOne("BusinessPortal.Domain.Entities.Location.State", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("BusinessPortal.Domain.Entities.Account.Role", b =>
                {
                    b.Navigation("RolePermissions");

                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("BusinessPortal.Domain.Entities.Account.User", b =>
                {
                    b.Navigation("Addresses");

                    b.Navigation("Advertisements");

                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("BusinessPortal.Domain.Entities.Address.Address", b =>
                {
                    b.Navigation("Advertisement");
                });

            modelBuilder.Entity("BusinessPortal.Domain.Entities.Advertisement.Advertisement", b =>
                {
                    b.Navigation("AdvertisementSelectedCategory");

                    b.Navigation("AdvertisementTags");
                });

            modelBuilder.Entity("BusinessPortal.Domain.Entities.BrowseCategory.Category", b =>
                {
                    b.Navigation("AdvertisementSelectedCategory");

                    b.Navigation("Categories");
                });

            modelBuilder.Entity("BusinessPortal.Domain.Entities.Location.State", b =>
                {
                    b.Navigation("AddressesCity");

                    b.Navigation("AddressesCountry");

                    b.Navigation("AddressesState");

                    b.Navigation("Children");
                });
#pragma warning restore 612, 618
        }
    }
}
