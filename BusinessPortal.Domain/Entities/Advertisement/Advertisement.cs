using BusinessPortal.Domain.Entities.Account;
using BusinessPortal.Domain.Entities.Common;
using BusinessPortal.Domain.Entities.Location;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPortal.Domain.Entities.Advertisement
{
    public class Advertisement : BaseEntity
    {
        #region Properties

        public ulong? AddressId { get; set; }

        public ulong UserId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public AdvertisementStatus AdvertisementStatus { get; set; }

        [Display(Name = "تصویر آگهی")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string ImageName { get; set; }

        public int VisitCount { get; set; }

        public bool FromCustomer { get; set; }

        public bool FromEmployee { get; set; }

        public string? DeclineMessage { get; set; }

        public ulong? CountriesId { get; set; }

        public bool OurOffer { get; set; }

        #endregion

        #region Relations

        [ForeignKey("UserId")]
        public User User { get; set; }

        [ForeignKey("AddressId")]
        public State State { get; set; }

        public List<AdvertisementSelectedCategory> AdvertisementSelectedCategory { get; set; }

        public ICollection<AdvertisementTag> AdvertisementTags { get; set; }

        public ICollection<AdvertisementInfo> AdvertisementInfo { get; set; }

        public Countries.Countries Countries { get; set; }

        #endregion
    }

    public enum AdvertisementStatus
    {
        [Display(Name = "تایید شده ")]
        Active,
        [Display(Name = "در انتظار بررسی")]
        WaitigForConfirm,
        [Display(Name = "تایید نشده ")]
        NotApproved
    }
}
