using MediatR;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate;

namespace OzonEdu.MerchandiseService.Domain.Events
{

    public class MerchReservedOnStockDomainEvent : INotification
    {
        public MerchandiseRequest MerchRequest { get; }

        public MerchReservedOnStockDomainEvent(MerchandiseRequest merchRequest)
        {
            MerchRequest = merchRequest;
        }
    }
}
