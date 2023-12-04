using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using GameStore.Shared.DTOs.Genre;
using Northwind.Data.Entities;

namespace Northwind.Services.MappingProfiles;

[ExcludeFromCodeCoverage]
public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, GenreFullDto>()
            .ForMember(
                dest => dest.Name,
                opts => opts.MapFrom(src => src.CategoryName));

        CreateMap<Category, GenreBriefDto>()
            .ForMember(
                dest => dest.Name,
                opts => opts.MapFrom(src => src.CategoryName));
    }
}