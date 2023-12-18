using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using GameStore.Shared.DTOs.Order;
using Northwind.Data.Entities;

namespace Northwind.Services.MappingProfiles;

[ExcludeFromCodeCoverage]
public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<Order, OrderBriefDto>();
        CreateMap<OrderDetail, OrderDetailDto>();
    }
}