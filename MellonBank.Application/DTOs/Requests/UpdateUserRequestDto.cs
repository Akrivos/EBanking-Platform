namespace MellonBank.Application.DTOs.Requests
{
    public record UpdateUserRequestDto(
        string FirstName,
        string LastName,
        string Address,
        string PhoneNumber,
        string Email
    );
}
