using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.Util;

namespace GameStore.Shared.Validators.GameValidators;

[ExcludeFromCodeCoverage]
public class GameCreateValidator : AbstractValidator<GameCreateDto>
{
    public GameCreateValidator()
    {
        RuleFor(g => g.Game.Key)
            .Must(x => !EntityAliasUtil.ContainsSuffix(x))
            .WithMessage($"{nameof(GameCreateDto.Game.Key)} cannot end with '{EntityAliasUtil.AliasSuffix}'.");
        RuleFor(g => g.Game.Name)
            .NotEmpty()
            .Must(x => !EntityAliasUtil.ContainsSuffix(x))
            .WithMessage($"{nameof(GameCreateDto.Game.Name)} cannot end with '{EntityAliasUtil.AliasSuffix}'.");
        RuleFor(g => g.Game.Price).GreaterThanOrEqualTo(0M);
        RuleFor(g => g.Game.Discontinued)
            .Must(x => x is 0 or 1)
            .WithMessage($"'{nameof(GameCreateDto.Game)} {nameof(GameCreateDto.Game.Discontinued)}' must be either 0 or 1.");
        RuleFor(g => g.Game.UnitInStock).GreaterThanOrEqualTo((short)0);
    }
}