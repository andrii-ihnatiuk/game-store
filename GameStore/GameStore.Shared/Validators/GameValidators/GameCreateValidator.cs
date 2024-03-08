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
        RuleFor(g => g.Game.Discount).InclusiveBetween((ushort)0, (ushort)100);
        RuleFor(g => g.Game.UnitInStock).GreaterThanOrEqualTo((short)0);

        When(g => g.Images is not null && g.Images.Count > 0, () =>
        {
            RuleFor(g => g.Images)
                .Must(images => images!.Count(img => img.IsCover).Equals(1))
                .WithMessage("Game must contain exactly one cover image.");
        });
    }
}