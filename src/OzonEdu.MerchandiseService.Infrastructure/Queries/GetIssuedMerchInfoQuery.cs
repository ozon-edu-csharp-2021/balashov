using System.Collections.Generic;
using MediatR;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate;

namespace OzonEdu.MerchandiseService.Infrastructure.Queries
{
    public class GetIssuedMerchInfoQuery : IRequest<List<MerchandiseRequest>>
    {
        public long EmployeeId { get; set; }
    }
}
