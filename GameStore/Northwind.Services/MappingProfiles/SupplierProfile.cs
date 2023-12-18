using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using GameStore.Shared.DTOs.Publisher;
using GameStore.Shared.Util;
using Northwind.Data.Entities;

namespace Northwind.Services.MappingProfiles;

[ExcludeFromCodeCoverage]
public class SupplierProfile : Profile
{
    public SupplierProfile()
    {
        CreateMap<Supplier, PublisherFullDto>()
            .ForMember(
                dest => dest.CompanyName,
                opts => opts.MapFrom(src => EntityAliasUtil.AddSuffix(src.CompanyName)))
            .ForMember(
                dest => dest.Description,
                opts => opts.MapFrom(src => string.Empty));

        CreateMap<Supplier, PublisherBriefDto>()
            .ForMember(
                dest => dest.CompanyName,
                opts => opts.MapFrom(src => EntityAliasUtil.AddSuffix(src.CompanyName)))
            .ForMember(
                dest => dest.Description,
                opts => opts.MapFrom(src => string.Empty));

        CreateMap<Supplier, PublisherUpdateDto>()
            .ForMember(
                dest => dest.Publisher,
                opts => opts.MapFrom(src => src));
        CreateMap<Supplier, PublisherUpdateInnerDto>()
            .ForMember(
                dest => dest.Description,
                opts => opts.MapFrom(src => string.Empty))
            .ForMember(
                dest => dest.HomePage,
                opts => opts.MapFrom(src => src.HomePage ?? string.Empty));
    }
}