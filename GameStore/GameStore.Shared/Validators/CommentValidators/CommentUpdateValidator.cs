using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using GameStore.Shared.DTOs.Comment;

namespace GameStore.Shared.Validators.CommentValidators;

[ExcludeFromCodeCoverage]
public class CommentUpdateValidator : AbstractValidator<CommentUpdateDto>
{
    public CommentUpdateValidator()
    {
        RuleFor(c => c.Comment.Body)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(c => c.Comment.Name)
            .NotEmpty()
            .MaximumLength(50);
    }
}