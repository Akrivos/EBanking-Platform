using FluentValidation;
using EBanking.Application.DTOs.Requests;

namespace EBanking.Application.Validators
{
    public sealed class CreateBankAccountRequestDtoValidator : AbstractValidator<CreateBankAccountRequestDto>
    {
        public CreateBankAccountRequestDtoValidator()
        {
            RuleFor(x => x.CustomerAfm)
                .NotEmpty()
                .Length(9);

            RuleFor(x => x.InitialBalance)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Branch)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Currency)
                .IsInEnum();

            RuleFor(x => x.AccountType)
                .IsInEnum();
        }
    }
}
