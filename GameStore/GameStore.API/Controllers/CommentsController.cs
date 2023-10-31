using GameStore.Services.Interfaces;
using GameStore.Shared.DTOs.Comment;
using GameStore.Shared.Validators;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentService _commentService;
    private readonly IValidatorWrapper<CommentCreateDto> _commentCreateValidator;

    public CommentsController(ICommentService commentService, IValidatorWrapper<CommentCreateDto> commentCreateValidator)
    {
        _commentService = commentService;
        _commentCreateValidator = commentCreateValidator;
    }

    [HttpGet("{gameAlias}")]
    public async Task<ActionResult<IList<CommentBriefDto>>> GetCommentsByGameAliasAsync(string gameAlias)
    {
        return Ok(await _commentService.GetCommentsByGameAliasAsync(gameAlias));
    }

    [HttpPost("{gameAlias}/new")]
    public async Task<IActionResult> AddCommentToGameAsync([FromRoute] string gameAlias, [FromBody] CommentCreateDto commentCreateDto)
    {
        _commentCreateValidator.ValidateAndThrow(commentCreateDto);
        var result = await _commentService.AddCommentToGameAsync(gameAlias, commentCreateDto);
        return Ok(result);
    }
}