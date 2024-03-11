﻿using GameStore.Data.Extensions;
using GameStore.Services.Interfaces;
using GameStore.Shared.DTOs.User;
using GameStore.Shared.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ProfileController : ControllerBase
{
    private readonly IProfileService _profileService;
    private readonly IValidatorWrapper<ContactInfoUpdateDto> _validator;

    public ProfileController(
        IProfileService profileService,
        IValidatorWrapper<ContactInfoUpdateDto> validator)
    {
        _profileService = profileService;
        _validator = validator;
    }

    [Authorize]
    [HttpGet("contacts")]
    public async Task<ActionResult<ContactInfoDto>> GetContactInfo()
    {
        var info = await _profileService.GetContactInfoAsync(this.GetAuthorizedUserId());
        return Ok(info);
    }

    [Authorize]
    [HttpPut("contacts")]
    public async Task<IActionResult> UpdateContactInfo([FromBody] ContactInfoUpdateDto dto)
    {
        _validator.ValidateAndThrow(dto);
        await _profileService.UpdateContactInfoAsync(dto);
        return NoContent();
    }
}