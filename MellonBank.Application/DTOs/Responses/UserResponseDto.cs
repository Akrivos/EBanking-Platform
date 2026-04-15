namespace MellonBank.Application.DTOs.Responses
{
    public record UserResponseDto(
        string FirstName,
        string LastName,
        string Afm,
        string PhoneNumber,
        string Email,
        string UserName,
        string Address
    );
}
