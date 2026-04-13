using System.ComponentModel.DataAnnotations;

namespace MellonBank.Web.Areas.Staff.ViewModels.Users
{
    public class UpdateUserParamViewModel
    {
        [Required]
        public string Afm { get; set; }
    }
}
