namespace MellonBank.Application.DTOs.Responses
{
    public record UserResponseDto(
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
