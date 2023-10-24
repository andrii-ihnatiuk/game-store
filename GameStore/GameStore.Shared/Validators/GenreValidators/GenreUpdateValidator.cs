using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using GameStore.Shared.DTOs.Genre;

namespace GameStore.Shared.Validators.GenreValidators;

[ExcludeFromCodeCoverage]
public class GenreUpdateValidator : AbstractValidator<GenreUpdateDto>
{
    public GenreUpdateValidator()
    {
        RuleFor(g => g.Genre.Name)
            .NotEmpty()
            .MaximumLength(40);
    }
}