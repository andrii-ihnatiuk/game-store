using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using GameStore.Shared.DTOs.Game;

namespace GameStore.Shared.Validators.GameValidators;

[ExcludeFromCodeCoverage]
public class GamesFilterValidator : AbstractValidator<GamesFilterDto>
{
    public GamesFilterValidator()
    {
        RuleFor(f => f.MaxPrice)
            .Must(v => v is null or > 0)
            .WithMessage("MaxPrice should be either null or > 0!");

        RuleFor(f => f.MinPrice)
            .Must(v => v is null or >= 0)
            .WithMessage("MaxPrice should be either null or >= 0!");

        When(f => !string.IsNullOrEmpty(f.Name), () =>
        {
            RuleFor(f => f.Name)
                .MinimumLength(3);
        });

        When(f => f.MaxPrice is not null && f.MinPrice is not null, () =>
        {
            RuleFor(r => r.MaxPrice)
                .GreaterThan(r => r.MinPrice);
        });
    }
}