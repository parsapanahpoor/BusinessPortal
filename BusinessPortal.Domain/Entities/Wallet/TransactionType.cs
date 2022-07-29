using System.ComponentModel.DataAnnotations;

namespace BusinessPortal.Domain.Entities.Wallet;

public enum TransactionType
{
    [Display(Name = "واریز")]
    Deposit = 0,

    [Display(Name = "برداشت")]
    Withdraw = 1,
}