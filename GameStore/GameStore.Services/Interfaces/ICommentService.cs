using GameStore.Shared.DTOs.Comment;

namespace GameStore.Services.Interfaces;

public interface ICommentService
{
    Task<IList<CommentBriefDto>> GetCommentsByGameAliasAsync(string gameAlias);

    Task<IList<CommentBriefDto>> AddCommentToGameAsync(string gameAlias, CommentCreateDto commentDto);

    Task DeleteCommentAsync(Guid commentId);
}