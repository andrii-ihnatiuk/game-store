using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using GameStore.Shared.DTOs.Publisher;

namespace GameStore.Shared.Validators.PublisherValidators;

[ExcludeFromCodeCoverage]
public class PublisherCreateValidator : AbstractValidator<PublisherCreateDto>
{
    public PublisherCreateValidator()
    {
        RuleFor(p => p.Publisher.CompanyName)
            .NotEmpty()
            .MaximumLength(40);
        RuleFor(p => p.Publisher.Description).NotEmpty();
        RuleFor(p => p.Publisher.HomePage).NotEmpty();
    }
}