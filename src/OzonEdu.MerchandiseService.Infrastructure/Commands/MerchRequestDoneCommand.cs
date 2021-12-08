using MediatR;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate;

namespace OzonEdu.MerchandiseService.Infrastructure.Commands
{

    public class MerchRequestDoneCommand : IRequest<MerchandiseRequest>
    {
        public long MerchRequestId { get; set; }
    }
}
