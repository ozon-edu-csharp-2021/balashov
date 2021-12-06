using System;
using System.Reflection.Metadata.Ecma335;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OzonEdu.MerchandiseService.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchandiseService.Domain.Contracts;
using OzonEdu.MerchandiseService.Domain.DomainServices;
using OzonEdu.MerchandiseService.Infrastructure.Commands;

namespace OzonEdu.MerchandiseService.Infrastructure.Handlers.Commands
{
    class RequestMerchCommandHandler : IRequestHandler<RequestMerchCommand, MerchandiseRequest>
    {
        private readonly IMerchRepository _merchRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmployeeRepository _employeeRepository;

        public RequestMerchCommandHandler(
            IMerchRepository merchRepository, 
            IUnitOfWork unitOfWork,
            IEmployeeRepository employeeRepository)
        {
            _merchRepository = merchRepository;
            _unitOfWork = unitOfWork;
            _employeeRepository = employeeRepository;
        }

        public async Task<MerchandiseRequest> Handle(RequestMerchCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.StartTransaction(cancellationToken); //нужно навсети порядок с unitOfWork

            //Взять работника
            var employee = await _employeeRepository.FindByIdAsync(request.EmployeeId, cancellationToken);
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
