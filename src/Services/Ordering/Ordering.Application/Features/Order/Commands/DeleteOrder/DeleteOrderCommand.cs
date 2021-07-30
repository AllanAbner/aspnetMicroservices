using MediatR;

namespace Ordering.Application.Features.Order.Commands.DeleteOrder
{
    public class DeleteOrderCommand : IRequest
    {
        public int Id { get; set; }
    }
}