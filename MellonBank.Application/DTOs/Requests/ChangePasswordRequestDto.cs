namespace MellonBank.Application.DTOs.Requests
{
    public record ChangePasswordRequestDto(
        string CurrentPassword,
        string NewPassword,
        string ConfirmNewPassword
    );
}
