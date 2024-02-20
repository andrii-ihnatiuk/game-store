using AutoMapper;
using GameStore.Data.Entities;
using GameStore.Data.Entities.Identity;
using GameStore.Data.Extensions;
using GameStore.Data.Interfaces;
using GameStore.Services.Interfaces;
using GameStore.Shared.DTOs.User;
using GameStore.Shared.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Services;

public class ProfileService : IProfileService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProfileService(
        UserManager<ApplicationUser> userManager,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ContactInfoDto> GetContactInfoAsync(string userId)
    {
        var user = await _userManager.Users
            .Where(u => u.Id.Equals(userId))
            .Include(u => u.NotificationMethods).ThenInclude(x => x.NotificationMethod)
            .FirstOrDefaultAsync() ?? throw new EntityNotFoundException(userId);

        return _mapper.Map<ContactInfoDto>(user);
    }

    public async Task UpdateContactInfoAsync(ContactInfoUpdateDto dto)
    {
        var user = await _userManager.Users
            .Where(u => u.Id.Equals(dto.Id))
            .Include(u => u.NotificationMethods).ThenInclude(x => x.NotificationMethod)
            .FirstOrDefaultAsync() ?? throw new EntityNotFoundException(dto.Id);

        var names = dto.NotificationMethods.Select(x => x.Name.ToUpperInvariant());
        var notificationMethods = await _unitOfWork.NotificationMethods.GetAsync(
            predicate: x => names.Contains(x.NormalizedName));

        if (notificationMethods.Count != dto.NotificationMethods.Count)
        {
            throw new ForeignKeyException(nameof(UserNotificationMethod.NotificationMethodId));
        }

        _mapper.Map(dto, user);
        user.NotificationMethods = notificationMethods.Select(x => new UserNotificationMethod
        {
            UserId = user.Id,
            NotificationMethodId = x.Id,
        }).ToList();

        await _userManager.UpdateAsync(user).ThrowIfFailedAsync();
    }
}