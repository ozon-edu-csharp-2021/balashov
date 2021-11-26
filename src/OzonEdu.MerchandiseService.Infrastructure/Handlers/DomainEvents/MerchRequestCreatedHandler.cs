using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ManagerAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchandiseService.Domain.Contracts;
using OzonEdu.MerchandiseService.Domain.Events;

namespace OzonEdu.MerchandiseService.Infrastructure.Handlers.DomainEvents
{
    public class MerchRequestCreatedHandler : INotificationHandler<MerchRequestCreatedDomainEvent>
    {
        private readonly IManagerRepository _managerRepository;
        private readonly IMerchRepository _merchRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MerchRequestCreatedHandler(IManagerRepository managerRepository, IMerchRepository merchRepository, IUnitOfWork unitOfWork)
        {
            _managerRepository = managerRepository;
            _merchRepository = merchRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(MerchRequestCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            await _unitOfWork.StartTransaction(cancellationToken);

            var merchRequest = notification.MerchRequest;
            var date = new Date(DateTime.Now);

            var manager = await ManagerProcessing.GetTheManagerOrFreeManagerAsync(_managerRepository, merchRequest.HRManagerId, cancellationToken);
            if (manager == null)
            {
                var text = merchRequest.HRManagerId > 0
                    ? $"Не удалось найти менеджера с id:{merchRequest.HRManagerId}"
                    : "Не удалось найти свободного менеджера";
                throw new Exception(text);
            }

            manager.AssignTask();
            await _managerRepository.UpdateAssignedTasksAsync(manager);

            merchRequest.AddManagerInfo(manager.Id, manager.PhoneNumber);
            
            if(!merchRequest.SetAssigned(date))
                throw new Exception($"Заявка id:{merchRequest.Id} не назначена HR-менеджеру! Метод вернул отказ");

            await _merchRepository.UpdateAsync(merchRequest, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
