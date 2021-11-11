using System;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseService.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ManagerAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate;
using OzonEdu.MerchandiseService.Domain.DomainServices;
using OzonEdu.MerchandiseService.Infrastructure.Commands;

namespace OzonEdu.MerchandiseService.Infrastructure.Handlers
{
    class RequestMerchCommandHandler : IRequestHandler<RequestMerchCommand, MerchandiseRequest>
    {
        private readonly IMerchRepository _merchRepository;
        private readonly IManagerRepository _managerRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public RequestMerchCommandHandler(IMerchRepository merchRepository, IManagerRepository managerRepository, IEmployeeRepository employeeRepository)
        {
            _merchRepository = merchRepository;
            _managerRepository = managerRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task<MerchandiseRequest> Handle(RequestMerchCommand request, CancellationToken cancellationToken)
        {
            //Взять работника
            var employee = await _employeeRepository.FindByIdAsync(request.HRManagerId, cancellationToken);

            //Проверить что такой мерч еще не выдавался сотруднику
            var allMerchRequestForEmployee = await _merchRepository.FindByEmployeeIdAsync(request.EmployeeId, cancellationToken);
            var issuedMerchs = allMerchRequestForEmployee.FindAll(mr => mr.RequestedMerchPack.Equals(request.RequestedMerchPack));
            if (!MerchDomainService.IsEmployeeReceivedMerchLastTime(issuedMerchs, employee, request.Date, out string whyString))
            {
               throw new Exception(whyString);
            }

            //Сформировать заявку у указанного менеджера, если не указан, то у любого свободного
            var merchRequest = await CreateMerchRequest(request, employee, cancellationToken);

            await _merchRepository.CreateAsync(merchRequest, cancellationToken);

            //Проверяется наличие данного мерча на складе через запрос к stock - api
            //TODO: Тут должен быть запрос к stock - api после того, как мы изучим эту тему
            if (true)
            {
                merchRequest.SetInProgress(request.Date);
                
                await _merchRepository.UpdateAsync(merchRequest, cancellationToken);

                //Если все проверки прошли - резервируется мерч в stock - api
                //TODO: Тут должен быть ещё один запрос к stock - api после того, как мы изучим эту тему
                if (true)
                    merchRequest.SetDone(request.Date);
            }

            //Выслать е-mail
            //TODO: Тут должен быть ещё один запрос к е-mail сервису после того, как мы изучим эту тему

            //Зафиксировать в БД, что сотруднику выдан мерч
            await _merchRepository.UpdateAsync(merchRequest, cancellationToken);

            await _merchRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            //Вернуть заявку результат 
            return merchRequest;
        }

        private async Task<MerchandiseRequest> CreateMerchRequest(
            RequestMerchCommand request,
            Employee employee,
            CancellationToken cancellationToken)
        {
            if (request.HRManagerId > 0)
            {
                var manager = await _managerRepository.FindByIdAsync(request.HRManagerId, cancellationToken);
                return MerchDomainService.CreateMerchandiseRequest(manager, employee, request.RequestedMerchPack);
            }
            else
            {
                var managers = await _managerRepository.GetAll(cancellationToken);
                return MerchDomainService.CreateMerchandiseRequest(managers, employee, request.RequestedMerchPack);
            }
        }
    }
}
