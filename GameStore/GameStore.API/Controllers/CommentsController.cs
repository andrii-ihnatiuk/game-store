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

    [HttpGet]
    [Route("/ban/durations")]
    public IActionResult GetBanDurationsAsync()
    {
        return Ok(_commentService.GetBanDurations());
    }

    [HttpPost]
    [Route("/ban")]
    public IActionResult BanUser(BanUserDto banDto)
    {
        _commentService.BanUser(banDto);
        return Ok();
    }

    [HttpPost("{gameAlias}/new")]
    public async Task<ActionResult<IList<CommentBriefDto>>> AddCommentToGameAsync([FromRoute] string gameAlias, [FromBody] CommentCreateDto commentCreateDto)
    {
        _commentCreateValidator.ValidateAndThrow(commentCreateDto);
        var result = await _commentService.AddCommentToGameAsync(gameAlias, commentCreateDto);
        return Ok(result);
    }

    [HttpDelete("{gameAlias}/remove/{commentId:guid}")]
    public async Task<IActionResult> DeleteCommentAsync(Guid commentId)
    {
        await _commentService.DeleteCommentAsync(commentId);
        return NoContent();
    }
}