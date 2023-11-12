using AutoMapper;
using GameStore.Data.Entities;
using GameStore.Data.Interfaces;
using GameStore.Services.Interfaces;
using GameStore.Shared.Constants;
using GameStore.Shared.DTOs.Comment;
using GameStore.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace GameStore.Services;

public class CommentService : ICommentService
{
    private const string OnDeleteMessage = "A comment/quote was deleted";
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _memoryCache;

    public CommentService(IUnitOfWork unitOfWork, IMapper mapper, IMemoryCache memoryCache)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _memoryCache = memoryCache;
    }

    public async Task<IList<CommentBriefDto>> GetCommentsByGameAliasAsync(string gameAlias)
    {
        var comments = (await _unitOfWork.Comments.GetAsync(
                predicate: c => c.Game.Alias == gameAlias,
                noTracking: false))
            .Where(c => c.ParentId == null);
        return _mapper.Map<IList<CommentBriefDto>>(comments);
    }

    public IList<string> GetBanDurations()
    {
        return new[] { "1 hour", "1 day", "1 week", "1 month", "Permanent", };
    }

    public void BanUser(BanUserDto banDto)
    {
        var bannedUsers = _memoryCache.TryGetValue("banned_users", out Dictionary<string, DateTime> dict)
            ? dict
            : new Dictionary<string, DateTime>();
        var banEndDate = GetBanEndDate(banDto.Duration);
        bannedUsers[banDto.User] = banEndDate;
        _memoryCache.Set("banned_users", bannedUsers);
    }

    public async Task<IList<CommentBriefDto>> AddCommentToGameAsync(string gameAlias, CommentCreateDto commentDto)
    {
        ThrowIfUserBanned(commentDto.Comment.Name);

        var game = await _unitOfWork.Games.GetOneAsync(
            predicate: g => g.Alias == gameAlias,
            noTracking: false);

        var comment = _mapper.Map<Comment>(commentDto);
        await DefineCommentIntroByTypeAsync(comment, gameAlias);
        game.Comments.Add(comment);
        await _unitOfWork.SaveAsync();

        var updatedComments = await GetCommentsByGameAliasAsync(gameAlias);
        return _mapper.Map<IList<CommentBriefDto>>(updatedComments);
    }

    public async Task DeleteCommentAsync(Guid commentId)
    {
        var comment = await _unitOfWork.Comments.GetOneAsync(
            predicate: c => c.Id == commentId,
            include: q => q.Include(c => c.Replies.Where(r => r.Type == CommentType.Quote)),
            noTracking: false);

        comment.Body = OnDeleteMessage;
        UpdateIntoForQuotingCommentsAsync(comment);
        await _unitOfWork.SaveAsync();
    }

    private async Task DefineCommentIntroByTypeAsync(Comment comment, string gameAlias)
    {
        if (comment.ParentId is null)
        {
            return;
        }

        var parent = await _unitOfWork.Comments.GetOneAsync(c => c.Id == comment.ParentId);
        comment.Intro = comment.Type switch
        {
            CommentType.Quote => BuildQuoteInto(parent.Body),
            CommentType.Reply => $"<a href=\"games/{gameAlias}#comment{parent.Id}\">[{parent.Author}]</a>&nbsp;",
            CommentType.Plain => string.Empty,
            _ => string.Empty,
        };
    }

    private static void UpdateIntoForQuotingCommentsAsync(Comment comment)
    {
        foreach (var reply in comment.Replies)
        {
            reply.Intro = BuildQuoteInto(OnDeleteMessage);
        }
    }

    private static string BuildQuoteInto(string message)
    {
        return $"<i>«{message}»</i><br/>";
    }

    private static DateTime GetBanEndDate(string durationStr)
    {
        var dateTime = DateTime.UtcNow;
        return durationStr switch
        {
            "1 hour" => dateTime.AddHours(1),
            "1 day" => dateTime.AddDays(1),
            "1 week" => dateTime.AddDays(7),
            "1 month" => dateTime.AddMonths(1),
            "Permanent" => DateTime.MaxValue,
            _ => DateTime.MinValue,
        };
    }

    private void ThrowIfUserBanned(string user)
    {
        if (_memoryCache.Get("banned_users") is not Dictionary<string, DateTime> bannedUsers)
        {
            return;
        }

        var banEndDate = bannedUsers.TryGetValue(user, out DateTime date) ? date : DateTime.MinValue;
        if (DateTime.UtcNow < banEndDate)
        {
            throw new UserBannedException($"You are banned to write comments until {banEndDate:dd/MM/yyyy HH:mm}");
        }
    }
}