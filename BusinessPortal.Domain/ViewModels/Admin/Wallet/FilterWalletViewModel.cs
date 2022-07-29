using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;
using BusinessPortal.Domain.Entities.Wallet;
using BusinessPortal.Domain.ViewModels.Common;

namespace BusinessPortal.Domain.ViewModels.Admin.Wallet;

public class FilterWalletViewModel: BasePaging<BusinessPortal.Domain.Entities.Wallet.Wallet>
{
    #region Filter Properties

    public ulong? UserId { get; set; }

    [DisplayName("User")]
    public string? UserFilter { get; set; }

    [DisplayName("Transaction Type")]
    public TransactionType? TransactionType { get; set; }

    [DisplayName("Gateway Type")]
    public GatewayType? GatewayType { get; set; }

    [DisplayName("Payment Type")]
    public PaymentType? PaymentType { get; set; }

    [DisplayName("Min Price")]
    public int? MinPrice { get; set; }

    [DisplayName("Max Price")]
    public int? MaxPrice { get; set; }

    [DisplayName("From Date")]
    public DateTime? MinCreateDate { get; set; }

    [DisplayName("To Date")]
    public DateTime? MaxCreateDate { get; set; }

    [DisplayName("Description")]
    public string? Description { get; set; }

    [DisplayName("Deleted ?")]
    public bool? IsDelete { get; set; }

    #endregion

    #region Order Properties

    [DisplayName("Sort By")]
    public FilterWalletOrderType OrderType { get; set; }

    #endregion

    public enum FilterWalletOrderType
    {
        [Display(Name = "تاریخ نزولی")]
        CreateDateDesc = 1, 

        [Display(Name = "تاریخ صعودی")]
        CreateDate = 2,

        [Display(Name = "قیمت صعودی")]
        Price = 3, 
        
        [Display(Name = "قیمت نزولی")]
        PriceDesc = 4, 
    }
}