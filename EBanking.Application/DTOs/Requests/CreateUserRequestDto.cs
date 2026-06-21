using EBanking.Domain.Enums;

namespace EBanking.Application.DTOs.Requests
{
    public sealed record CreateUserRequestDto(
        string FirstName,
        string LastName,
        string Afm,
        string Address,
        string PhoneNumber,
        string Email,
        string UserName,
        string Password,
        RoleType Role
    );
}
