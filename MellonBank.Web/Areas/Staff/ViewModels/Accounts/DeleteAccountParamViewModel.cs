using System.ComponentModel.DataAnnotations;

namespace MellonBank.Web.Areas.Staff.ViewModels.Accounts
{
    public class DeleteAccountParamViewModel
    {
        [Required]
        public string AccountNumber { get; set; } = string.Empty;
    }
}
