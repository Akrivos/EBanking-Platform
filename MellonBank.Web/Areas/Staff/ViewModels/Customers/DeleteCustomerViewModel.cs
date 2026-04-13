using System.ComponentModel.DataAnnotations;

namespace MellonBank.Web.Areas.Staff.ViewModels.Customers
{
    public class DeleteCustomerViewModel
    {
        [Required]
        public string AFM { get; set; }
    }
}
