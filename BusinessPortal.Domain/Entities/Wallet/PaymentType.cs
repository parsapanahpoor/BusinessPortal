using System.ComponentModel.DataAnnotations;

namespace BusinessPortal.Domain.Entities.Wallet;

public enum PaymentType
{
    [Display(Name = "شارژ کیف پول")]
    ChargeWallet = 0,

    [Display(Name = "خرید")]
    Buy = 1,
}