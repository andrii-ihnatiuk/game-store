using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using GameStore.Shared.DTOs.Payment;

namespace GameStore.Shared.Validators.PaymentValidators;

[ExcludeFromCodeCoverage]
public class PaymentValidator : AbstractValidator<PaymentDto>
{
    public PaymentValidator()
    {
        RuleFor(p => p.Method)
            .NotEmpty()
            .MaximumLength(50);

        When(p => p.Model is not null, () =>
        {
            RuleFor(p => p.Model.Holder)
                .MinimumLength(1)
                .WithMessage("Card holder must be at least one character in length.");

            RuleFor(p => p.Model.CardNumber)
                .Length(16)
                .WithMessage("Card number must be 16 characters in length.")
                .Must(p => p.StartsWith("4", StringComparison.InvariantCulture))
                .WithMessage("Visa card number must start with 4.");

            RuleFor(p => p.Model.MonthExpire)
                .InclusiveBetween((ushort)1, (ushort)12)
                .WithMessage("Month number must be between 1 and 12");

            RuleFor(p => p.Model.Cvv2)
                .InclusiveBetween(1U, 999U)
                .WithMessage("CVV value must be between 1 and 999.");
        });
    }
}