using System;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseService.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchandiseService.Domain.Contracts;
using OzonEdu.MerchandiseService.Domain.DomainServices;
using OzonEdu.MerchandiseService.Infrastructure.Commands;
using OzonEdu.MerchandiseService.Infrastructure.InterfacesToExternals;

namespace OzonEdu.MerchandiseService.Infrastructure.Handlers
{
    class RequestMerchCommandHandler : IRequestHandler<RequestMerchCommand, MerchandiseRequest>
    {
        private readonly IMerchRepository _merchRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmployeeServer _employeeServer;

        public RequestMerchCommandHandler(
            IMerchRepository merchRepository, 
            IUnitOfWork unitOfWork, 
            IEmployeeServer employeeServer)
        {
            _merchRepository = merchRepository;
            _unitOfWork = unitOfWork;
            _employeeServer = employeeServer;
        }

        public async Task<MerchandiseRequest> Handle(RequestMerchCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.StartTransaction(cancellationToken);

            //Взять работника
            var employee = await _employeeServer.GetByIdAsync(request.EmployeeId, cancellationToken);
            if(employee == null)
                throw new Exception($"Запрашиваемый сотрудник не обнаружен id:{request.EmployeeId}");

            //Проверить что такой мерч еще не выдавался сотруднику
            var allMerchRequestForEmployee = await _merchRepository.FindByEmployeeIdAsync(employee.Id, cancellationToken);
            var canReceive = MerchDomainService.CanEmployeeReceiveNewMerch(
                allMerchRequestForEmployee, 
                request.RequestedMerchPack, 
                employee, 
                request.Date,
                out string whyNotString);

            if (!canReceive)
               throw new Exception(whyNotString);

            //Сформировать новую заявку
            var merchRequest = MerchandiseRequestFactory.NewMerchandiseRequest(
                request.HRManagerId,
                employee, 
                request.Size, 
                request.RequestedMerchPack, 
                new Date(DateTime.Now));

            if (!merchRequest.SetCreated(new Date(DateTime.Now)))
                throw new Exception("Заявка не сформирована! Метод вернул отказ");

            await _merchRepository.CreateAsync(merchRequest, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
           
            return merchRequest;
        }
    }
}
