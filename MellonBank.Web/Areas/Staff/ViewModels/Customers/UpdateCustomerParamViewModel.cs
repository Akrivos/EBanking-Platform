using System.ComponentModel.DataAnnotations;

namespace MellonBank.Web.Areas.Staff.ViewModels.Customers
{
    public class UpdateCustomerParamViewModel
    {
        [Required]
        public string Afm { get; set; }
    }
}
