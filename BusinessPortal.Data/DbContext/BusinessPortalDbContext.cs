using BusinessPortal.Domain.Entities.Account;
using BusinessPortal.Domain.Entities.SiteSetting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessPortal.Domain.Entities.BrowseCategory;
using BusinessPortal.Domain.Entities.Location;
using BusinessPortal.Domain.Entities.Advertisement;
using BusinessPortal.Domain.Entities.Address;
using BusinessPortal.Domain.Entities.Language;
using BusinessPortal.Domain.Entities.Wallet;
using BusinessPortal.Domain.Entities.Tariff;
using BusinessPortal.Domain.Entities.Contact;
using BusinessPortal.Domain.Entities.Services;
using System.Globalization;
using BusinessPortal.Domain.Entities.Product;
using BusinessPortal.Domain.Entities.Countries;
using BusinessPortal.Domain.Entities.Ads;

namespace BusinessPortal.Data.DbContext
{
    public class BusinessPortalDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        #region Ctor

        public BusinessPortalDbContext(DbContextOptions<BusinessPortalDbContext> options) : base(options)
        {
        }

        #endregion

        #region DbSets

        #region Language

        public DbSet<Language> Language { get; set; }

        #endregion

        #region Account 

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RequestForSeller> requestForSellers { get; set; }
        public DbSet<Seller> Seller { get; set; }

        #endregion

        #region Brows Categories

        public DbSet<Category> Categories { get; set; }

        #endregion

        #region Advertisement

        public DbSet<Advertisement> Advertisement { get; set; }

        public DbSet<AdvertisementInfo> advertisementInfo { get; set; }

        public DbSet<AdvertisementTag> AdvertisementTags { get; set; }

        public DbSet<AdvertisementSelectedCategory> AdvertisementSelectedCategories { get; set; }

        public DbSet<UserCreateAdvertisementLog> UserCreateAdvertisementLogs { get; set; }

        public DbSet<UserSeenAdvertisementLog> UserSeenAdvertisementLogs { get; set; }

        #endregion

        #region Email Setting

        public DbSet<EmailSetting> EmailSettings { get; set; }

        #endregion

        #region Location && Address

        public DbSet<State> States { get; set; }

        public DbSet<Address> Addresses { get; set; }

        #endregion

        #region Wallet

        public DbSet<Wallet> Wallets { get; set; }

        public DbSet<WalletData> WalletData { get; set; }

        #endregion

        #region Tariff 

        public DbSet<Tariff> Tariffs { get; set; }

        public DbSet<UserSelectedTariff> UserSelectedTariff { get; set; }

        #endregion

        #region Ticket 

        public DbSet<Ticket> Tickets { get; set; }

        public DbSet<TicketMessage> TicketMessages { get; set; }

        #endregion

        #region Product

        public DbSet<ProductCategory> ProductCategories { get; set; }

        public DbSet<ProductCategoryInfo> ProductCategoryInfos { get; set; }

        #endregion

        #region Categories

        public DbSet<ServicesCategory> ServicesCategories { get; set; }

        public DbSet<ServicesCategoryInfo> ServicesCategoryInfos { get; set; }

        #endregion

        #region Ads 

        public DbSet<Ads> Ads { get; set; }

        public DbSet<AdsGallery> AdsGalleries { get; set; }

        public DbSet<AdsInfo> AdsInfo { get; set; }

        #endregion

        #region Countries

        public DbSet<Countries> Countries { get; set; }

        #endregion

        #endregion

        #region On Model Creating

        public string culture = CultureInfo.CurrentCulture.Name;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            #region query filter

            modelBuilder.Entity<ServicesCategoryInfo>().HasQueryFilter(ac => ac.LanguageId == culture);
            modelBuilder.Entity<ProductCategoryInfo>().HasQueryFilter(ac => ac.LanguageId == culture);

            #endregion

            #region Seed Data

            #region Languages

            modelBuilder.Entity<Language>().HasData(new Language
            {
                  LanguageTitle = "fa-IR"
            });

            modelBuilder.Entity<Language>().HasData(new Language
            {
                LanguageTitle = "en-US"
            });

            modelBuilder.Entity<Language>().HasData(new Language
            {
                LanguageTitle = "ar-SA"
            });

            modelBuilder.Entity<Language>().HasData(new Language
            {
                LanguageTitle = "tr-TR"
            });

            modelBuilder.Entity<Language>().HasData(new Language
            {
                LanguageTitle = "ru-RU"
            });

            modelBuilder.Entity<Language>().HasData(new Language
            {
                LanguageTitle = "pt-PT"
            });

            #endregion

            #region Email Setting Seed Data

            var date = new DateTime(2022, 03, 01);

            modelBuilder.Entity<EmailSetting>().HasData(new EmailSetting
            {
                Id = 1,
                Password = "54511441",
                IsDelete = false,
                CreateDate = date,
                IsDefaultEmail = true,
                DisplayName = "BusinessPortal",
                From = "parsapanahpoor77@gmail.com",
                Smtp = "smtp.gmail.com",
                EnableSsL = true,
                Port = 587,
                UserName = "BusinessPortal"
            });

            #endregion

            #endregion

            base.OnModelCreating(modelBuilder);
        }

        #endregion
    }
}
