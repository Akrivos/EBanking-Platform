using FluentValidation;
using MellonBank.Application.DTOs.Requests;

namespace MellonBank.Application.Validators
{
    public class TransferToThirdPartyRequestDtoValidator : AbstractValidator<TransferToThirdPartyRequestDto>
    {
        public TransferToThirdPartyRequestDtoValidator()
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
