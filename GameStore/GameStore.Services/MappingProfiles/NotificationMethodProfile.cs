using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using GameStore.Data.Entities;
using GameStore.Shared.DTOs.Notification;

namespace GameStore.Services.MappingProfiles;

[ExcludeFromCodeCoverage]
public class NotificationMethodProfile : Profile
{
    public NotificationMethodProfile()
    {
        CreateMap<NotificationMethod, NotificationMethodDto>();
    }
}