using FluentValidation;
using GameStore.Shared.DTOs.Platform;

namespace GameStore.Shared.Validators.PlatformValidators;

public class PlatformCreateValidator : AbstractValidator<PlatformCreateDto>
{
    public PlatformCreateValidator()
    {
        RuleFor(p => p.Platform.Type)
            .NotEmpty()
            .MaximumLength(40);
    }
}