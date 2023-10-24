﻿using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using GameStore.Shared.DTOs.Genre;

namespace GameStore.Shared.Validators.GenreValidators;

[ExcludeFromCodeCoverage]
public class GenreCreateValidator : AbstractValidator<GenreCreateDto>
{
    public GenreCreateValidator()
    {
        RuleFor(g => g.Genre.Name)
            .NotEmpty()
            .MaximumLength(40);
    }
}