using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Order.Commands.CheckoutOrder
{
    public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IMapper mapper;
        private readonly IEmailService emailService;
        private readonly ILogger<CheckoutOrderCommandHandler> logger;

        public CheckoutOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, IEmailService emailService, ILogger<CheckoutOrderCommandHandler> logger)
        {
            this.orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = mapper.Map<Domain.Entities.Order>(request);
            var order = await orderRepository.AddAsync(orderEntity);

            logger.LogInformation($"Order {order.Id} is successfully created");

            await SendEmail(order);

            return order.Id;
        }

        private async Task SendEmail(Domain.Entities.Order order)
        {
            var email = new Email
            {
                To = order.UserName,
                Body = "Order was created.",
                Subject = "Order was created"
            };

            try
            {
                await emailService.SendEmail(email);
            }
            catch (Exception ex)
            {
                logger.LogError($"Order {order.Id} failed due to an error with the mail service: {ex.Message}");
            }
        }
    }
}