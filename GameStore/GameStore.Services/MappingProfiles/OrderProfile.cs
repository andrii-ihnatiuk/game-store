using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using GameStore.Data.Entities;
using GameStore.Shared.DTOs.Order;

namespace GameStore.Services.MappingProfiles;

[ExcludeFromCodeCoverage]
public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<OrderDetail, OrderDetailDto>();

        CreateMap<Order, OrderBriefDto>();
    }
}