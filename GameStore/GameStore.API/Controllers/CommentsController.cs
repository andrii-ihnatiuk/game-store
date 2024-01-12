using GameStore.API.Attributes;
using GameStore.Services.Interfaces;
using GameStore.Shared.Constants;
using GameStore.Shared.DTOs.Comment;
using GameStore.Shared.Exceptions;
using GameStore.Shared.Util;
using GameStore.Shared.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentService _commentService;
    private readonly IValidatorWrapper<CommentCreateDto> _commentCreateValidator;
    private readonly IValidatorWrapper<CommentUpdateDto> _commentUpdateValidator;
    private readonly IAuthorizationService _authorizationService;

    public CommentsController(
        ICommentService commentService,
        IValidatorWrapper<CommentCreateDto> commentCreateValidator,
        IValidatorWrapper<CommentUpdateDto> commentUpdateValidator,
        IAuthorizationService authorizationService)
    {
        _commentService = commentService;
        _commentCreateValidator = commentCreateValidator;
        _commentUpdateValidator = commentUpdateValidator;
        _authorizationService = authorizationService;
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

    [HasAnyPermission(PermissionOptions.CommentBan, PermissionOptions.CommentFull)]
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

        if (User.Identity.IsAuthenticated && !User.Identity.Name.Equals(commentCreateDto.Comment.Name, StringComparison.InvariantCulture))
        {
            return BadRequest("You must specify your username as the author!");
        }

        if (EntityAliasUtil.ContainsSuffix(gameAlias))
        {
            throw new GameStoreNotSupportedException("Commenting on Northwind products is not supported!");
        }

        var authResult = await _authorizationService.AuthorizeAsync(User, gameAlias, PolicyNames.CanCommentOnGame);
        if (!authResult.Succeeded)
        {
            return Forbid();
        }

        var result = await _commentService.AddCommentToGameAsync(gameAlias, commentCreateDto);
        return Ok(result);
    }

    [HasAnyPermission(PermissionOptions.CommentUpdate, PermissionOptions.CommentUpdateOnDeleted, PermissionOptions.CommentFull)]
    [HttpPut("update")]
    public async Task<IActionResult> UpdateCommentAsync(CommentUpdateDto dto)
    {
        _commentUpdateValidator.ValidateAndThrow(dto);

        var authResult = await _authorizationService.AuthorizeAsync(User, dto, PolicyNames.CanUpdateComment);
        if (!authResult.Succeeded)
        {
            return Forbid();
        }

        await _commentService.UpdateCommentAsync(dto);
        return NoContent();
    }

    [HasAnyPermission(PermissionOptions.CommentDelete, PermissionOptions.CommentFull)]
    [HttpDelete("{gameAlias}/remove/{commentId:guid}")]
    public async Task<IActionResult> DeleteCommentAsync(Guid commentId)
    {
        await _commentService.DeleteCommentAsync(commentId);
        return NoContent();
    }
}