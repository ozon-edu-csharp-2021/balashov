using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OzonEdu.MerchandiseService.Domain.AggregationModels.Enumerations;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate;
using OzonEdu.MerchandiseService.Infrastructure.Queries;

namespace OzonEdu.MerchandiseService.Infrastructure.Handlers.Queries
{
    public class GetIssuingMerchInfoQueryHandler : IRequestHandler<GetIssuingMerchInfoQuery, List<MerchandiseRequest>>
    {
        private readonly IMerchRepository _merchRepository;

        public GetIssuingMerchInfoQueryHandler(IMerchRepository merchRepository)
        {
            _merchRepository = merchRepository ?? throw new ArgumentNullException(nameof(merchRepository));
        }

        public async Task<List<MerchandiseRequest>> Handle(GetIssuingMerchInfoQuery request, CancellationToken cancellationToken)
        {
            var allMerchRequestForEmployee = await _merchRepository.FindByEmployeeIdAsync(request.EmployeeId, cancellationToken);
            var issuedMerch = allMerchRequestForEmployee.FindAll(mr => !mr.Status.Status.Equals(MerchRequestStatusType.Done));

            return issuedMerch;
        }
    }
}
