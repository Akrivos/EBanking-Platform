using FluentValidation;
using EBanking.Application.DTOs.Requests;

namespace EBanking.Application.Validators
{
    public sealed class TransferToOwnAccountRequestDtoValidator : AbstractValidator<TransferToOwnAccountRequestDto>
    {
        public TransferToOwnAccountRequestDtoValidator()
        {
            RuleFor(x => x.FromAccountNumber)
                .NotEmpty()
                .MaximumLength(20);

            RuleFor(x => x.ToAccountNumber)
                .NotEmpty()
                .MaximumLength(20);

            RuleFor(x => x.Amount)
                .GreaterThan(0);

            RuleFor(x => x)
                .Must(dto => dto.FromAccountNumber != dto.ToAccountNumber)
                .WithMessage("Source and destination account numbers must be different.");
        }
    }
}
