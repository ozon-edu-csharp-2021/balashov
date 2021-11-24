using System;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseService.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.Enumerations;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ManagerAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchandiseService.Domain.Contracts;
using OzonEdu.MerchandiseService.Domain.DomainServices;
using OzonEdu.MerchandiseService.Infrastructure.Commands;
using OzonEdu.MerchandiseService.Infrastructure.Interfaces;
using OzonEdu.MerchandiseService.Infrastructure.InterfacesToExternal;

namespace OzonEdu.MerchandiseService.Infrastructure.Handlers
{
    class RequestMerchCommandHandler : IRequestHandler<RequestMerchCommand, MerchandiseRequest>
    {
        private readonly IMerchRepository _merchRepository;
        private readonly IManagerRepository _managerRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailServer _emailServer;
        private readonly IStockApiServer _stockApiServer;

        public RequestMerchCommandHandler(IMerchRepository merchRepository, IManagerRepository managerRepository, IEmployeeRepository employeeRepository, IUnitOfWork unitOfWork, IEmailServer emailServer, IStockApiServer stockApiServer)
        {
            _merchRepository = merchRepository;
            _managerRepository = managerRepository;
            _employeeRepository = employeeRepository;
            _unitOfWork = unitOfWork;
            _emailServer = emailServer;
            _stockApiServer = stockApiServer;
        }

        public async Task<MerchandiseRequest> Handle(RequestMerchCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.StartTransaction(cancellationToken);

            //Взять работника TODO вынести в отдельный обработчик. Если работника нет, то сформировать его и вернуть
            var employee = await _employeeRepository.FindByIdAsync(request.EmployeeId, cancellationToken);
            if(employee == null)
                throw new Exception("Запрашиваемый сотрудник не обнаружен");

            //Проверить что такой мерч еще не выдавался сотруднику
            var allMerchRequestForEmployee = await _merchRepository.FindByEmployeeIdAsync(employee.Id, cancellationToken);
            if (!MerchDomainService.CanEmployeeReceiveNewMerch(
                allMerchRequestForEmployee, request.RequestedMerchPack, employee, request.Date, out string whyNotString))
            {
               throw new Exception(whyNotString);
            }

            //Взять указанного менеджера, если не указан, то у любого свободного
            var manager = await ManagerProcessing.GetManager(_managerRepository, request.HRManagerId, cancellationToken);

            //Сформировать заявку
            var merchRequest = MerchandiseRequestFactory.Create(
                manager, 
                employee, 
                request.Size, 
                request.RequestedMerchPack, 
                new Date(DateTime.Now));

            //Todo доменное событие, что сформирована новая заявка. Поставить счётчик задач на менеджера в +1
            merchRequest.SetAssigned(request.Date);

            await _merchRepository.CreateAsync(merchRequest, cancellationToken);

            //Проверяется наличие данного мерча на складе через запрос к stock - api
            //Если все проверки прошли - резервируется мерч в stock - api
            if (_stockApiServer.ReserveMerch(merchRequest))
                merchRequest.SetInProgress(request.Date);

            //TODO отметка что выдача произведена должна делается из отдельного метода API, тогда же снимать задачу с менеджера
            //if (true)
            //    merchRequest.SetDone(request.Date);

            await _merchRepository.UpdateAsync(merchRequest, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            //Выслать е-mail если резер произведён и ожидается получение мерча работником
            //TODO Стоит подумать про доменное событие, по которому отправляется письмо
            if(merchRequest.Status.Status.Equals(MerchRequestStatusType.InProgress))
                _emailServer.SendEmailAboutMerch(employee, merchRequest);

            return merchRequest;
        }
    }
}
