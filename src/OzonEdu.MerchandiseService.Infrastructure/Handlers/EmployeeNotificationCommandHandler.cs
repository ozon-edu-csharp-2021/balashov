using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate;
using OzonEdu.MerchandiseService.Infrastructure.Commands;

namespace OzonEdu.MerchandiseService.Infrastructure.Handlers
{
    public class EmployeeNotificationCommandHandler : IRequestHandler<EmployeeNotificationCommand, MerchandiseRequest>
    {
        private readonly ILogger<EmployeeNotificationCommandHandler> _logger;

        public EmployeeNotificationCommandHandler(ILogger<EmployeeNotificationCommandHandler> logger)
        {
            _logger = logger;
        }

        public Task<MerchandiseRequest> Handle(EmployeeNotificationCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Получено сообщение про нового сотрудника");

            return null;
        }
    }
}
