namespace EBanking.Application.DTOs.Requests
{
    public sealed record UpdateUserRequestDto(
        string FirstName,
        string LastName,
        string Address,
        string PhoneNumber,
        string Email
    );
}
