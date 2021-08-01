using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Exceptions;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Order.Commands.UpdateOrder
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IMapper mapper;

        private readonly ILogger<UpdateOrderCommandHandler> logger;

        public UpdateOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, ILogger<UpdateOrderCommandHandler> logger)
        {
            this.orderRepository = orderRepository ?? throw new System.ArgumentNullException(nameof(orderRepository));
            this.mapper = mapper ?? throw new System.ArgumentNullException(nameof(mapper));
            this.logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var orderToUpdate = await orderRepository.GetByIdAsync(request.Id);

            if (orderToUpdate == null)
            {
                if (orderToUpdate == null)
                {
                    logger.LogError($"Order {request.Id } not found in database.");
                    throw new NotFoundException(nameof(Domain.Entities.Order), request.Id);
                }
            }

            mapper.Map(request, orderToUpdate, typeof(UpdateOrderCommand), typeof(Domain.Entities.Order));

            await orderRepository.UpdateAsync(orderToUpdate);

            logger.LogInformation($"Order {orderToUpdate.Id} is successfully created");

            return Unit.Value;
        }
    }
}