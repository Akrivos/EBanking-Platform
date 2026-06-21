using FluentValidation;
using EBanking.Application.DTOs.Requests;

namespace EBanking.Application.Validators
{
    public sealed class UpdateBankAccountRequestDtoValidator : AbstractValidator<UpdateBankAccountRequestDto>
    {
        public UpdateBankAccountRequestDtoValidator()
        {
            RuleFor(x => x.Branch)
                .MaximumLength(100)
                .When(x => !string.IsNullOrEmpty(x.Branch));

            RuleFor(x => x.AccountType)
                .IsInEnum()
                .When(x => x.AccountType.HasValue);
        }
    }
}
