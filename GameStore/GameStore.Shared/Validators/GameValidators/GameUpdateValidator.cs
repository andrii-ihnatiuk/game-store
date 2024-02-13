﻿using System.Diagnostics.CodeAnalysis;
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
        RuleFor(g => g.Game.Discount).InclusiveBetween((ushort)0, (ushort)100);
        RuleFor(g => g.Game.UnitInStock).GreaterThanOrEqualTo((short)0);
    }
}