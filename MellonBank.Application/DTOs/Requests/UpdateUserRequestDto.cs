namespace MellonBank.Application.DTOs.Requests
{
    public record UpdateUserRequestDto(
        string FirstName,
        string LastName,
        string Afm,
        string Address,
        string PhoneNumber,
        string Email,
        string UserName,
        string Password
    );
}
