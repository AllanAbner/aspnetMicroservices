using AutoMapper;
using Ordering.Application.Features.Order.Commands.CheckoutOrder;
using Ordering.Application.Features.Order.Commands.UpdateOrder;
using Ordering.Application.Features.Order.Queries.GetOrderList;
using Ordering.Domain.Entities;

namespace Ordering.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Order, OrdersVm>().ReverseMap();
            CreateMap<Order, CheckoutOrderCommand>().ReverseMap();
            CreateMap<Order, UpdateOrderCommand>().ReverseMap();
        }
    }
}