using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.Util;
using Northwind.Data.Entities;

namespace Northwind.Services.MappingProfiles;

[ExcludeFromCodeCoverage]
public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, GameFullDto>()
            .ForMember(
                dest => dest.Key,
                opts => opts.MapFrom(src => EntityAliasUtil.AddSuffix(src.Alias)))
            .ForMember(
                dest => dest.Name,
                opts => opts.MapFrom(src => src.ProductName))
            .ForMember(
                dest => dest.Price,
                opts => opts.MapFrom(src => src.UnitPrice))
            .ForMember(
                dest => dest.UnitInStock,
                opts => opts.MapFrom(src => src.UnitsInStock));

        CreateMap<Product, GameBriefDto>()
            .ForMember(
                dest => dest.Key,
                opts => opts.MapFrom(src => EntityAliasUtil.AddSuffix(src.Alias)))
            .ForMember(
                dest => dest.Name,
                opts => opts.MapFrom(src => src.ProductName));
    }
}