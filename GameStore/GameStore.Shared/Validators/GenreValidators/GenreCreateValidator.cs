using FluentValidation;
using GameStore.Shared.DTOs.Genre;

namespace GameStore.Shared.Validators.GenreValidators;

public class GenreCreateValidator : AbstractValidator<GenreCreateDto>
{
    public GenreCreateValidator()
    {
        RuleFor(g => g.Genre.Name)
            .NotEmpty()
            .MaximumLength(40);
    }
}