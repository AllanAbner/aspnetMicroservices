using AutoMapper;
using EventBus.Messages.Events;
using Ordering.Application.Features.Order.Commands.CheckoutOrder;

namespace Ordering.API.Mappings
{
    public class OrderingProfile : Profile
    {
        public OrderingProfile()
        {
            CreateMap<BasketCheckoutEvent, CheckoutOrderCommand>().ReverseMap();
        }
    }
}