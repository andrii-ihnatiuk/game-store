using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using GameStore.Shared.DTOs.Platform;

namespace GameStore.Shared.Validators.PlatformValidators;

[ExcludeFromCodeCoverage]
public class PlatformUpdateValidator : AbstractValidator<PlatformUpdateDto>
{
    public PlatformUpdateValidator()
    {
        RuleFor(p => p.Platform.Type)
            .NotEmpty()
            .MaximumLength(40);
    }
}