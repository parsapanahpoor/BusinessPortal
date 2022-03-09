using BusinessPortal.Domain.Entities.Account;
using BusinessPortal.Domain.Entities.SiteSetting;
using BusinessPortal.Domain.Entities.Account;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessPortal.Domain.Entities.BrowseCategory;

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

        #region Account 

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        #endregion

        #region Brows Categories

        public DbSet<Category> Categories { get; set; }

        #endregion

        #region Email Setting

        public DbSet<EmailSetting> EmailSettings { get; set; }

        #endregion

        #endregion

        #region On Model Creating

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            #region Seed Data

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
