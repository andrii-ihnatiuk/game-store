using GameStore.API.Attributes;
using GameStore.Application.Interfaces.Util;
using GameStore.Services.Interfaces;
using GameStore.Shared.Constants;
using GameStore.Shared.DTOs.Role;
using GameStore.Shared.DTOs.User;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILoginServiceResolver _loginServiceResolver;

    public UsersController(IUserService userService, ILoginServiceResolver loginServiceResolver)
    {
        _userService = userService;
        _loginServiceResolver = loginServiceResolver;
    }

    [HasAnyPermission(PermissionOptions.UserView, PermissionOptions.UserFull)]
    [HttpGet]
    public async Task<ActionResult<IList<UserBriefDto>>> GetAllUsersAsync()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    [HasAnyPermission(PermissionOptions.UserView, PermissionOptions.UserFull)]
    [HttpGet("{userId}")]
    public async Task<ActionResult<UserBriefDto>> GetUserAsync(string userId)
    {
        var user = await _userService.GetUserByIdAsync(userId);
        return Ok(user);
    }

    [HasAnyPermission(PermissionOptions.UserView, PermissionOptions.UserFull)]
    [HttpGet("{userId}/roles")]
    public async Task<ActionResult<RoleBriefDto>> GetRolesByUserAsync(string userId)
    {
        var userRoles = await _userService.GetRolesByUserAsync(userId);
        return Ok(userRoles);
    }

    [HttpPost("check-access")]
    public IActionResult CheckAccess()
    {
        return Ok(true);
    }

    [HasAnyPermission(PermissionOptions.UserCreate, PermissionOptions.UserFull)]
    [HttpPost("register")]
    public async Task<ActionResult<UserBriefDto>> RegisterUserAsync([FromBody] UserRegistrationDto dto)
    {
        var registeredUser = await _userService.RegisterUserAsync(dto);
        return Ok(registeredUser);
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] UserLoginDto loginDto)
    {
        var loginService = _loginServiceResolver.Resolve(loginDto.Model);
        string token = await loginService.LoginAsync(loginDto);
        return Ok(new { token });
    }

    [HasAnyPermission(PermissionOptions.UserUpdate, PermissionOptions.UserFull)]
    [HttpPut("update")]
    public async Task<IActionResult> UpdateUserAsync([FromBody] UserUpdateDto dto)
    {
        await _userService.UpdateUserAsync(dto);
        return NoContent();
    }

    [HasAnyPermission(PermissionOptions.UserDelete, PermissionOptions.UserFull)]
    [HttpDelete("remove/{userId}")]
    public async Task<IActionResult> DeleteUserAsync([FromRoute] string userId)
    {
        await _userService.DeleteUserAsync(userId);
        return NoContent();
    }
}