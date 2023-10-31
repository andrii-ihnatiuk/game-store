using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using GameStore.Shared.DTOs.Comment;

namespace GameStore.Shared.Validators.CommentValidators;

[ExcludeFromCodeCoverage]
public class CommentCreateValidator : AbstractValidator<CommentCreateDto>
{
    public CommentCreateValidator()
    {
        RuleFor(c => c.Comment.Body)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(c => c.Comment.Name)
            .NotEmpty()
            .MaximumLength(50);
    }
}