using System.ComponentModel.DataAnnotations;

namespace MellonBank.Web.Areas.Staff.ViewModels.Accounts
{
    public class GetAccountParamViewModel
    {
        [Required]
        public string AccountNumber { get; set; }
    }
}
