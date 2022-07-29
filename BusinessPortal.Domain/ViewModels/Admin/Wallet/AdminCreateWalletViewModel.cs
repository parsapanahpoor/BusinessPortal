using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using BusinessPortal.Domain.Entities.Wallet;

namespace BusinessPortal.Domain.ViewModels.Admin.Wallet;

public class AdminCreateWalletViewModel
{
    #region Properties

    [DisplayName("کاربر")]
    [Required(ErrorMessage = "Please Enter {0}")]
    public ulong UserId { get; set; }

    [DisplayName("نوع تراکنش")]
    [Required(ErrorMessage = "Please Enter {0}")]
    public TransactionType TransactionType { get; set; }

    [DisplayName("درگاه پرداخت")]
    [Required(ErrorMessage = "Please Enter {0}")]
    public GatewayType GatewayType { get; set; }

    [DisplayName("علت تراکنش")]
    [Required(ErrorMessage = "Please Enter {0}")]
    public PaymentType PaymentType { get; set; }

    [DisplayName("مبلغ")]
    [Required(ErrorMessage = "Please Enter {0}")]
    public int Price { get; set; }

    [DisplayName("متن")]
    [AllowNull]
    [MaxLength(500, ErrorMessage = "Please Enter {0} Less Than {1} Character")]
    public string? Description { get; set; }

    #endregion

    #region Display Properties

    [AllowNull]
    public Entities.Account.User? User { get; set; }

    #endregion

}

public enum AdminCreateWalletResponse
{
    Success, UserNotFound, 
}