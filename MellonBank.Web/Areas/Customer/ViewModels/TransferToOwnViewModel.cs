using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MellonBank.Web.Areas.Customer.ViewModels
{
    public class TransferToOwnViewModel
    {
        [Required]
        [Display(Name = "From Account")]
        public string FromAccountNumber { get; set; } = string.Empty;

        [Required]
        [Display(Name = "To Account")]
        public string ToAccountNumber { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }

        public List<SelectListItem> AvailableAccounts { get; set; } = new();
    }
}
