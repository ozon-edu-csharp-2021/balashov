using MediatR;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate;

namespace OzonEdu.MerchandiseService.Domain.Events
{

    public class MerchRequestDoneDomainEvent : INotification
    {
        public MerchandiseRequest MerchRequest { get; }

        public MerchRequestDoneDomainEvent(MerchandiseRequest merchRequest)
        {
            MerchRequest = merchRequest;
        }
    }
}
