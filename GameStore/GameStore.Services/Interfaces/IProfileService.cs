using GameStore.Shared.DTOs.User;

namespace GameStore.Services.Interfaces;

public interface IProfileService
{
    Task<ContactInfoDto> GetContactInfoAsync(string userId);

    Task UpdateContactInfoAsync(ContactInfoUpdateDto dto);
}