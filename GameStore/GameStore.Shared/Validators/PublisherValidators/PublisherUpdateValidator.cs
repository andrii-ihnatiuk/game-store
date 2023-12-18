using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using GameStore.Shared.DTOs.Publisher;
using GameStore.Shared.Util;

namespace GameStore.Shared.Validators.PublisherValidators;

[ExcludeFromCodeCoverage]
public class PublisherUpdateValidator : AbstractValidator<PublisherUpdateDto>
{
    public PublisherUpdateValidator()
    {
        RuleFor(p => p.Publisher.CompanyName)
            .NotEmpty()
            .MaximumLength(40)
            .Must(x => !EntityAliasUtil.ContainsSuffix(x))
            .WithMessage($"{nameof(PublisherCreateDto.Publisher.CompanyName)} cannot end with '{EntityAliasUtil.AliasSuffix}'.");
        RuleFor(p => p.Publisher.Description).NotEmpty();
        RuleFor(p => p.Publisher.HomePage).NotEmpty();
    }
}