using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Exceptions;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Order.Commands.DeleteOrder
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
    {
        private readonly IOrderRepository orderRepository;
        private readonly ILogger<DeleteOrderCommandHandler> logger;

        public DeleteOrderCommandHandler(IOrderRepository orderRepository, ILogger<DeleteOrderCommandHandler> logger)
        {
            this.orderRepository = orderRepository;
            this.logger = logger;
        }

        public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var orderToDelete = await orderRepository.GetByIdAsync(request.Id);

            if (orderToDelete == null)
            {
                logger.LogError($"Order {request.Id } not found in database.");
                throw new NotFoundException(nameof(Order), request.Id);
            }

            await orderRepository.DeleteAsync(orderToDelete);

            logger.LogInformation($"Order {orderToDelete.Id} is successfully deleted");

            return Unit.Value;
        }
    }
}