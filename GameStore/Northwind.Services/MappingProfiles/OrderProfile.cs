using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using AutoMapper;
using GameStore.Shared.DTOs.Order;
using Northwind.Data.Entities;

namespace Northwind.Services.MappingProfiles;

[ExcludeFromCodeCoverage]
public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<Order, OrderBriefDto>()
            .ForMember(
                dest => dest.OrderDate,
                opts => opts.MapFrom(src => DateTime.ParseExact(src.OrderDate, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)));
    }
}