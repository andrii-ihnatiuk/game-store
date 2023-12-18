using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.Util;

namespace GameStore.Shared.Validators.GameValidators;

[ExcludeFromCodeCoverage]
public class GameUpdateValidator : AbstractValidator<GameUpdateDto>
{
    public GameUpdateValidator()
    {
        RuleFor(g => g.Game.Key)
            .Must(x => !EntityAliasUtil.ContainsSuffix(x))
            .WithMessage($"{nameof(GameUpdateDto.Game.Key)} cannot end with '{EntityAliasUtil.AliasSuffix}'.");
        RuleFor(g => g.Game.Name)
            .NotEmpty()
            .Must(x => !EntityAliasUtil.ContainsSuffix(x))
            .WithMessage($"{nameof(GameUpdateDto.Game.Name)} cannot end with '{EntityAliasUtil.AliasSuffix}'.");
        RuleFor(g => g.Game.Price).GreaterThanOrEqualTo(0M);
        RuleFor(g => g.Game.Discontinued)
            .Must(x => x is 0 or 1)
            .WithMessage($"'{nameof(GameUpdateDto.Game)} {nameof(GameUpdateDto.Game.Discontinued)}' must be either 0 or 1.");
        RuleFor(g => g.Game.UnitInStock).GreaterThanOrEqualTo((short)0);
    }
}