using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using GameStore.Shared.DTOs.Game;

namespace GameStore.Shared.Validators.GameValidators;

[ExcludeFromCodeCoverage]
public class GameCreateValidator : AbstractValidator<GameCreateDto>
{
    public GameCreateValidator()
    {
        RuleFor(g => g.Game.Name).NotEmpty();
        RuleFor(g => g.Game.Price).GreaterThanOrEqualTo(0M);
        RuleFor(g => g.Game.Discontinued)
            .Must(x => x is 0 or 1)
            .WithMessage($"'{nameof(GameCreateDto.Game)} {nameof(GameCreateDto.Game.Discontinued)}' must be either 0 or 1.");
        RuleFor(g => g.Game.UnitInStock).GreaterThanOrEqualTo((short)0);
    }
}