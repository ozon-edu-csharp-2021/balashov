using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OzonEdu.MerchandiseService.Domain.AggregationModels.Enumerations;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate;
using OzonEdu.MerchandiseService.Infrastructure.Queries;

namespace OzonEdu.MerchandiseService.Infrastructure.Handlers
{
    public class GetIssuedMerchInfoQueryHandler : IRequestHandler<GetIssuedMerchInfoQuery, List<MerchandiseRequest>>
    {
        private readonly IMerchRepository _merchRepository;

        public GetIssuedMerchInfoQueryHandler(IMerchRepository merchRepository)
        {
            _merchRepository = merchRepository ?? throw new ArgumentNullException(nameof(merchRepository)); ;
        }

        public async Task<List<MerchandiseRequest>> Handle(GetIssuedMerchInfoQuery request, CancellationToken cancellationToken)
        {
            var allMerchRequestForEmployee = await _merchRepository.FindByEmployeeIdAsync(request.EmployeeId, cancellationToken);
            var issuedMerch = allMerchRequestForEmployee.FindAll(mr => mr.Status.Status.Equals(MerchRequestStatusType.Done));

            return issuedMerch;
        }
    }
}
