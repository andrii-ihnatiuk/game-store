using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using GameStore.Shared.Constants;
using GameStore.Shared.DTOs.User;

namespace GameStore.Shared.Validators.UserValidators;

[ExcludeFromCodeCoverage]
public class ContactInfoUpdateValidator : AbstractValidator<ContactInfoUpdateDto>
{
    public ContactInfoUpdateValidator()
    {
        When(x => !string.IsNullOrEmpty(x.Email), () =>
        {
            RuleFor(x => x.Email)
                .EmailAddress();
        });

        When(x => x.NotificationMethods.Any(nm => nm.Name.Equals(NotificationMethods.Email)), () =>
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("No email address was specified to enable notification method.");
        });

        When(x => x.NotificationMethods.Any(nm => nm.Name.Equals(NotificationMethods.Sms)), () =>
        {
            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .WithMessage("No phone number was specified to enable SMS notification method.");
        });
    }
}