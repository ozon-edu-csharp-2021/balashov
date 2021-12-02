using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ManagerAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchandiseService.Domain.Contracts;
using OzonEdu.MerchandiseService.Infrastructure.Commands;

namespace OzonEdu.MerchandiseService.Infrastructure.Handlers
{
    public class MerchRequestDoneCommandHandler : IRequestHandler<MerchRequestDoneCommand, MerchandiseRequest>
    {
        private readonly IManagerRepository _managerRepository;
        private readonly IMerchRepository _merchRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MerchRequestDoneCommandHandler(IManagerRepository managerRepository, IMerchRepository merchRepository, IUnitOfWork unitOfWork)
        {
            _managerRepository = managerRepository;
            _merchRepository = merchRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<MerchandiseRequest> Handle(MerchRequestDoneCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.StartTransaction(cancellationToken);

            var merchRequest = await _merchRepository.FindByIdAsync(request.MerchRequestId, cancellationToken);

            var date = new Date(DateTime.Now);

            if (!merchRequest.SetDone(date))
                throw new Exception($"Не удалось подтвердить завершение выдачи id:{merchRequest.Id}! Метод вернул отказ");

            await _merchRepository.UpdateAsync(merchRequest, cancellationToken);

            var manager = await ManagerProcessing.GetTheManagerAsync(_managerRepository, merchRequest.HRManagerId, cancellationToken);
            if (manager == null)
            {
                var text = $"Не удалось найти менеджера с id:{merchRequest.HRManagerId}";
                throw new Exception(text);
            }

            manager.TaskDone();

            await _managerRepository.UpdateAssignedTasksAsync(manager, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return merchRequest;
        }
    }
}
