﻿using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using GameStore.Shared.DTOs.Platform;

namespace GameStore.Shared.Validators.PlatformValidators;

[ExcludeFromCodeCoverage]
public class PlatformCreateValidator : AbstractValidator<PlatformCreateDto>
{
    public PlatformCreateValidator()
    {
        RuleFor(p => p.Platform.Type)
            .NotEmpty()
            .MaximumLength(40);
    }
}