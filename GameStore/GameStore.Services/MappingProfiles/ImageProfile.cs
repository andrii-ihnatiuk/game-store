using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using GameStore.Data.Entities;
using GameStore.Shared.DTOs.Image;

namespace GameStore.Services.MappingProfiles;

[ExcludeFromCodeCoverage]
public class ImageProfile : Profile
{
    public ImageProfile()
    {
        CreateMap<AppImage, ImageBriefDto>();
    }
}