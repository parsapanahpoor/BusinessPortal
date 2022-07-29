using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BusinessPortal.Domain.Entities.Wallet;

public enum GatewayType
{
    [Display(Name = "زرین پال")]
    Zarinpal = 0,

    [Display(Name = "ادمین سیستم")]
    System = 1,
    
    [Display(Name = "پی پال")]
    PayPal = 2,

    [Display(Name = "استرایپ")]
    Stripe = 3 
}