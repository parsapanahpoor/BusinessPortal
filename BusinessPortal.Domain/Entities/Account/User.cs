﻿using BusinessPortal.Domain.Entities.Account;
using BusinessPortal.Domain.Entities.Ads;
using BusinessPortal.Domain.Entities.Advertisement;
using BusinessPortal.Domain.Entities.Common;
using BusinessPortal.Domain.Entities.Contact;
using BusinessPortal.Domain.Entities.Location;
using BusinessPortal.Domain.Entities.Product;
using BusinessPortal.Domain.Entities.Services;
using BusinessPortal.Domain.Entities.Tariff;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.Entities.Account
{
    public class User : BaseEntity
    {
        #region Properties

        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "این فیلد الزامی است .")]
        [MaxLength(300, ErrorMessage = "تعداد کاراکتر های {0} نمیتواند بیشتر از {1} باشد")]
        public string Username { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "این فیلد الزامی است .")]
        [MaxLength(150, ErrorMessage = "تعداد کاراکتر های {0} نمیتواند بیشتر از {1} باشد")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نیست .")]
        public string Email { get; set; }

        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "این فیلد الزامی است .")]
        [MaxLength(100, ErrorMessage = "تعداد کاراکتر های {0} نمیتواند بیشتر از {1} باشد")]
        public string Password { get; set; }

        [Display(Name = "تلفن همراه")]
        [MaxLength(20, ErrorMessage = "تعداد کاراکتر های {0} نمیتواند بیشتر از {1} باشد")]
        public string? Mobile { get; set; }

        [Display(Name = "آواتار")]
        [MaxLength(100, ErrorMessage = "تعداد کاراکتر های {0} نمیتواند بیشتر از {1} باشد")]
        public string? Avatar { get; set; }

        [Display(Name = "کد فعالسازی ایمیل")]
        [Required(ErrorMessage = "این فیلد الزامی است .")]
        [MaxLength(100, ErrorMessage = "تعداد کاراکتر های {0} نمیتواند بیشتر از {1} باشد")]
        public string EmailActivationCode { get; set; }

        [Display(Name = "کد فعالسازی موبایل")]
        [Required(ErrorMessage = "این فیلد الزامی است .")]
        [MaxLength(100, ErrorMessage = "تعداد کاراکتر های {0} نمیتواند بیشتر از {1} باشد")]
        public string? MobileActivationCode { get; set; }

        public bool IsMobileConfirm { get; set; } = false;

        public bool IsEmailConfirm { get; set; } = false;

        public bool IsBan { get; set; } = false;

        public bool IsAdmin { get; set; } = false;

        public bool BanForTicket { get; set; }

        public bool BanForComment { get; set; }

        public int WalletBalance { get; set; }

        #endregion

        #region Relations

        public ICollection<UserRole> UserRoles { get; set; }

        public ICollection<Address.Address> Addresses { get; set; }

        public ICollection<Advertisement.Advertisement> Advertisements { get; set; }

        public ICollection<ProductService> ProductServices { get; set; }

        public ICollection<Product.Product> Products { get; set; }

        public RequestForSeller RequestForSeller { get; set; }

        public Seller Seller { get; set; }

        public List<Wallet.Wallet> Wallets { get; set; }

        public List<UserSelectedTariff> UserSelectedTariff { get; set; }

        public ICollection<UserSeenAdvertisementLog> UserSeenAdvertisementLogs { get; set; }

        public ICollection<UserCreateAdvertisementLog> UserCreateAdvertisementLog { get; set; }

        public ICollection<Ticket> Tickets { get; set; }

        public ICollection<TicketMessage> TicketMessages { get; set; }

        public ICollection<Ads.Ads> Ads { get; set; }

        #endregion
    }
}
