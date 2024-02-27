﻿using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using GameStore.Data.Entities.Identity;
using GameStore.Shared.DTOs.User;

namespace GameStore.Services.MappingProfiles;

[ExcludeFromCodeCoverage]
public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<ApplicationUser, UserBriefDto>()
            .ForMember(
                dest => dest.Name,
                opts => opts.MapFrom(src => src.UserName));

        CreateMap<UserUpdateInnerDto, ApplicationUser>();
        CreateMap<UserUpdateDto, ApplicationUser>()
            .IncludeMembers(src => src.User)
            .ForMember(
                dest => dest.UserName,
                opts => opts.MapFrom(src => src.User.Name));
    }
}