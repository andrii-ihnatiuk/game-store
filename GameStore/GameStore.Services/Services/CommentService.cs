﻿using AutoMapper;
using GameStore.Data.Entities;
using GameStore.Data.Repositories;
using GameStore.Services.Interfaces;
using GameStore.Shared.Constants;
using GameStore.Shared.DTOs.Comment;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Services.Services;

public class CommentService : ICommentService
{
    private const string OnDeleteMessage = "A comment/quote was deleted";
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CommentService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IList<CommentBriefDto>> GetCommentsByGameAliasAsync(string gameAlias)
    {
        var comments = await _unitOfWork.Comments.GetAsync(
            predicate: c => c.Game.Alias == gameAlias && c.ParentId == null,
            include: q => q.Include(c => c.Replies));

        var repliesFlat = comments.SelectMany(c => c.Replies).ToList();
        await LoadRepliesTree(repliesFlat);
        return _mapper.Map<IList<CommentBriefDto>>(comments);
    }

    public async Task<IList<CommentBriefDto>> AddCommentToGameAsync(string gameAlias, CommentCreateDto commentDto)
    {
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

    private async Task LoadRepliesTree(IList<Comment> comments)
    {
        var parents = comments.Select(c => c.Id);

        // get the next level of replies, include their children to reduce amount of queries
        var replies = await _unitOfWork.Comments.GetAsync(
            predicate: c => parents.Contains((Guid)c.ParentId!),
            include: q => q.Include(c => c.Replies));

        // assign replies to their parent
        foreach (var reply in replies)
        {
            var foundParent = comments.First(c => c.Id == reply.ParentId);
            foundParent.Replies.Add(reply);
        }

        var children = replies.SelectMany(r => r.Replies).ToList();

        // load the next level of replies
        if (children.Count > 0)
        {
            await LoadRepliesTree(children);
        }
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
}