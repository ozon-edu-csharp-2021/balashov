using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OzonEdu.MerchandiseService.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.Enumerations;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate;
using OzonEdu.MerchandiseService.Infrastructure.Queries;

namespace OzonEdu.MerchandiseService.Infrastructure.Handlers.Queries
{
    public class GetIssuedMerchInfoQueryHandler : IRequestHandler<GetIssuedMerchInfoQuery, List<MerchandiseRequest>>
    {
        private readonly IMerchRepository _merchRepository;
        private readonly IEmployeeRepository _employeeRepository;


        public GetIssuedMerchInfoQueryHandler(IMerchRepository merchRepository, IEmployeeRepository employeeRepository)
        {
            _merchRepository = merchRepository ?? throw new ArgumentNullException(nameof(merchRepository));
            _employeeRepository = employeeRepository;
        }

        public async Task<List<MerchandiseRequest>> Handle(GetIssuedMerchInfoQuery request, CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository.FindByIdAsync(request.EmployeeId);
            if (employee == null)
                throw new Exception($"Сотрудник с id:{request.EmployeeId} в системе не обнаружен");

            var allMerchRequestForEmployee = await _merchRepository.FindByEmployeeEmailAsync(employee.Email.EmailString, cancellationToken);
            var issuedMerch = allMerchRequestForEmployee.FindAll(mr => mr.Status.Status.Equals(MerchRequestStatusType.Done));

            return issuedMerch;
        }
    }
}
