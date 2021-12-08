using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OzonEdu.MerchandiseService.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ManagerAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchandiseService.Domain.Contracts;
using OzonEdu.MerchandiseService.Domain.DomainServices;
using OzonEdu.MerchandiseService.Infrastructure.Commands;
using OzonEdu.MerchandiseService.Infrastructure.ExternalDataSources;

namespace OzonEdu.MerchandiseService.Infrastructure.Handlers.Commands
{
    class RequestMerchCommandHandler : IRequestHandler<RequestMerchCommand, MerchandiseRequest>
    {
        private readonly IMerchRepository _merchRepository;
        private readonly IManagerRepository _managerRepository;
        private readonly IStockApiServer _stockApiServer;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUnitOfWork _unitOfWork;


        public RequestMerchCommandHandler(
            IMerchRepository merchRepository, IUnitOfWork unitOfWork, IEmployeeRepository employeeRepository, 
            IStockApiServer stockApiServer, IManagerRepository managerRepository)
        {
            _merchRepository = merchRepository;
            _unitOfWork = unitOfWork;
            _employeeRepository = employeeRepository;
            _stockApiServer = stockApiServer;
            _managerRepository = managerRepository;
        }

        public async Task<MerchandiseRequest> Handle(RequestMerchCommand request, CancellationToken cancellationToken)
        {
            //Взять работника
            var employee = await _employeeRepository.FindByEmailAsync(request.EmployeeEmail, cancellationToken);
            if(employee == null)
                throw new Exception($"Запрашиваемый сотрудник не обнаружен email:{request.EmployeeEmail}");

            //Проверить что такой мерч еще не выдавался сотруднику
            var allMerchRequestForEmployee = await _merchRepository.FindByEmployeeEmailAsync(employee.Email.EmailString, cancellationToken);
            var canReceive = MerchDomainService.CanEmployeeReceiveNewMerch(allMerchRequestForEmployee, request.RequestedMerchPack, 
                request.Date, out string whyNotString);
            if (!canReceive)
               throw new Exception(whyNotString);

            //Сформировать новую заявку
            var merchRequest = MerchandiseRequestFactory.NewMerchandiseRequest(request.HRManagerId, employee, 
                request.Size, request.RequestedMerchPack, new Date(DateTime.Now));
            await _merchRepository.CreateAsync(merchRequest, cancellationToken);

            //Назначить менеджера
            await MerchDomainService.MerchRequestAssignTaskForHr(merchRequest, _managerRepository, cancellationToken);
            await _merchRepository.UpdateAsync(merchRequest, cancellationToken);

            //Проверяется наличие/зарезервировать мерч на складе через запрос к stock - api
            //Если все проверки прошли - резервируется мерч в stock - api
            if (_stockApiServer.ReserveMerch(merchRequest))
            {
                if (!merchRequest.SetReserved(new Date(DateTime.Now)))
                    throw new Exception($"Заявка id:{merchRequest.Id} не назначена в резерв! Метод вернул отказ");

                await _merchRepository.UpdateAsync(merchRequest, cancellationToken);
            }

            return merchRequest;
        }
    }
}
