using System;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseService.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.Enumerations;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ManagerAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate;
using OzonEdu.MerchandiseService.Domain.Contracts;
using OzonEdu.MerchandiseService.Domain.DomainServices;
using OzonEdu.MerchandiseService.Infrastructure.Commands;

namespace OzonEdu.MerchandiseService.Infrastructure.Handlers
{
    class RequestMerchCommandHandler : IRequestHandler<RequestMerchCommand, MerchandiseRequest>
    {
        private readonly IMerchRepository _merchRepository;
        private readonly IManagerRepository _managerRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUnitOfWork _unitOfWork;


        public RequestMerchCommandHandler(IMerchRepository merchRepository, IManagerRepository managerRepository, IEmployeeRepository employeeRepository, IUnitOfWork unitOfWork)
        {
            _merchRepository = merchRepository;
            _managerRepository = managerRepository;
            _employeeRepository = employeeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<MerchandiseRequest> Handle(RequestMerchCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.StartTransaction(cancellationToken);

            //Взять работника TODO вынести в отдельный обработчик. Если работника нет, то сформировать его и вернуть
            var employee = await _employeeRepository.FindByIdAsync(request.EmployeeId, cancellationToken);

            //Проверить что такой мерч еще не выдавался сотруднику
            var allMerchRequestForEmployee = await _merchRepository.FindByEmployeeIdAsync(request.EmployeeId, cancellationToken);
            var issuedMerchs = allMerchRequestForEmployee.FindAll(mr => mr.RequestedMerchPack.Equals(request.RequestedMerchPack));
            if (!MerchDomainService.IsEmployeeReceivedMerchLastTime(issuedMerchs, employee, request.Date, out string whyString))
            {
               throw new Exception(whyString);
            }

            //Сформировать заявку у указанного менеджера, если не указан, то у любого свободного
            var merchRequest = await CreateMerchRequest(request, employee, request.Size, cancellationToken);

            await _merchRepository.CreateAsync(merchRequest, cancellationToken);
            
            merchRequest.SetAssigned(request.Date);
            //Todo доменное событие, что сформирована новая заявка. Поставить счётчик задач на менеджера в +1

            //Проверяется наличие данного мерча на складе через запрос к stock - api
            //TODO: Тут должен быть запрос к stock - api
            if (true)
            {
                //Если все проверки прошли - резервируется мерч в stock - api
                //TODO: Тут должен быть ещё один запрос к stock - api
                merchRequest.SetInProgress(request.Date);
                
                await _merchRepository.UpdateAsync(merchRequest, cancellationToken);

                //TODO отметка что выдача произведена должна делается из отдельного метода API, тогда же снимать задачу с менеджера
                //if (true)
                //    merchRequest.SetDone(request.Date);
            }

            //Выслать е-mail
            //TODO: Тут должен быть ещё один запрос к е-mail сервису

            //Зафиксировать в БД, что сотруднику выдан мерч
            await _merchRepository.UpdateAsync(merchRequest, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            //Вернуть заявку результат 
            return merchRequest;
        }

        private async Task<MerchandiseRequest> CreateMerchRequest(
            RequestMerchCommand request,
            Employee employee,
            Size size,
            CancellationToken cancellationToken)
        {
            if (request.HRManagerId > 0)
            {
                var manager = await _managerRepository.FindByIdAsync(request.HRManagerId, cancellationToken);
                return MerchandiseRequestFactory.CreateMerchandiseRequest(manager, employee, size, request.RequestedMerchPack);
            }
            else
            {
                var managers = await _managerRepository.GetAllAsync(cancellationToken);
                return MerchandiseRequestFactory.CreateMerchandiseRequest(managers, employee, size, request.RequestedMerchPack);
            }
        }
    }
}
