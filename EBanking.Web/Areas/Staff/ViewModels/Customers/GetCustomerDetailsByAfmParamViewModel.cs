using System.ComponentModel.DataAnnotations;

namespace EBanking.Web.Areas.Staff.ViewModels.Customers
{
    public class GetCustomerDetailsByAfmParamViewModel
    {
        [Required]
        public string Afm { get; set; } = string.Empty;
    }
}
