namespace MellonBank.Application.DTOs.Responses
{
    public sealed record UserResponseDto(
        string Id,
        string FirstName,
        string LastName,
        string Afm,
        string UserName,
        string Email
    );
}
