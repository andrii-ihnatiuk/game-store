using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using GameStore.Shared.DTOs.Publisher;
using Northwind.Data.Entities;
using Northwind.Services.Util;

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
    }
}