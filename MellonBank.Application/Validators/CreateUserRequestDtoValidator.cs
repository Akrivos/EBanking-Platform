using FluentValidation;
using MellonBank.Application.Common.Validation;
using MellonBank.Application.DTOs.Requests;

namespace MellonBank.Application.Validators
{
    public class CreateUserRequestDtoValidator : AbstractValidator<CreateUserRequestDto>
    {
        public CreateUserRequestDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Afm)
                .NotEmpty()
                .Matches(ValidationPatterns.Afm).WithMessage(ValidationMessages.Afm);

            RuleFor(x => x.Address)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .MaximumLength(16);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.UserName)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(6)
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"\d").WithMessage("Password must contain at least one number.")
                .Matches(@"[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");

            RuleFor(x => x.Role)
                .NotEmpty()
                .IsInEnum();
        }
    }
}
