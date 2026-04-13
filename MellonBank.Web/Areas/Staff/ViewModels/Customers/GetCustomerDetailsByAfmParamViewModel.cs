using System.ComponentModel.DataAnnotations;

namespace MellonBank.Web.Areas.Staff.ViewModels.Users
{
    public class GetUserByAfmParamViewModel
    {
        [Required]
        public string Afm { get; set; }
    }
}
