using FluentValidation;
using EBanking.Application.DTOs.Requests;

namespace EBanking.Application.Validators
{
    public sealed class ChangePasswordRequstDtoValidator : AbstractValidator<ChangePasswordRequestDto>
    {
        public ChangePasswordRequstDtoValidator()
        {
            RuleFor(x => x.CurrentPassword)
                .NotEmpty()
                .MinimumLength(6);

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .MinimumLength(6)
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"\d").WithMessage("Password must contain at least one number.")
                .Matches(@"[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");

            RuleFor(x => x.ConfirmNewPassword)
                .NotEmpty()
                .Equal(x => x.NewPassword).WithMessage("Passwords do not match.");

            RuleFor(x => x)
                .Must(x => x.CurrentPassword != x.NewPassword)
                .WithMessage("New password must be different from current password.");
        }
    }
}
