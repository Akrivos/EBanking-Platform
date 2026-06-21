namespace EBanking.Application.DTOs.Requests
{
    public sealed record ChangePasswordRequestDto(
        string CurrentPassword,
        string NewPassword,
        string ConfirmNewPassword
    );
}
