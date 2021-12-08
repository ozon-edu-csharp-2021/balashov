using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OzonEdu.MerchandiseService.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.Enumerations;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ManagerAggregate;
using OzonEdu.MerchandiseService.Domain.Events;
using OzonEdu.MerchandiseService.Infrastructure.ExternalDataSources;

namespace OzonEdu.MerchandiseService.Infrastructure.Handlers.DomainEvents
{
    public class MerchRequestReservedHandler : INotificationHandler<MerchReservedOnStockDomainEvent>
    {
        private readonly IEmailServer _emailServer;
        private readonly IManagerRepository _managerRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public MerchRequestReservedHandler(
            IEmailServer emailServer, 
            IManagerRepository managerRepository, 
            IEmployeeRepository employeeRepository)
        {
            _emailServer = emailServer;
            _managerRepository = managerRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task Handle(MerchReservedOnStockDomainEvent notification, CancellationToken cancellationToken)
        {
            var merchRequest = notification.MerchRequest;

            if (!merchRequest.Status.Status.Equals(MerchRequestStatusType.Reserved))
                return;

            var employee = await _employeeRepository.FindByEmailAsync(merchRequest.EmployeeEmail, cancellationToken);
            if (employee == null)
                throw new Exception($"Запрашиваемый сотрудник не обнаружен email:{merchRequest.EmployeeEmail}");

            var manager = await ManagerProcessing.GetTheManagerAsync(_managerRepository, merchRequest.HRManagerId, cancellationToken);
            if (manager == null)
                throw new Exception($"Не удалось найти менеджера с id:{merchRequest.HRManagerId}");

            await _emailServer.SendEmailAboutMerchAsync(employee, manager, merchRequest);
        }
    }
}
