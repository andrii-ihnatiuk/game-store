using GameStore.Shared.DTOs.Comment;

namespace GameStore.Application.Interfaces;

public interface ICommentService
{
    Task<IList<CommentBriefDto>> GetCommentsByGameAliasAsync(string gameAlias);

    IList<string> GetBanDurations();

    void BanUser(BanUserDto banDto);

    Task<IList<CommentBriefDto>> AddCommentToGameAsync(string gameAlias, CommentCreateDto commentDto);

    Task DeleteCommentAsync(Guid commentId);
}