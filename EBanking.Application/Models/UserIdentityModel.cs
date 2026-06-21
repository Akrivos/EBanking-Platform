namespace EBanking.Application.Models
{
    public sealed record UserIdentityModel(
            string Id,
            string FirstName,
            string LastName,
            string Address,
            string Afm,
            string PhoneNumber,
            string Email,
            string UserName
        );
}
