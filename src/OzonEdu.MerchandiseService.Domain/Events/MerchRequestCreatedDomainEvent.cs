using MediatR;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate;

namespace OzonEdu.MerchandiseService.Domain.Events
{

    public class MerchRequestCreatedDomainEvent : INotification
    {
        public MerchandiseRequest MerchRequest { get; }

        public MerchRequestCreatedDomainEvent(MerchandiseRequest merchRequest)
        {
            MerchRequest = merchRequest;
        }
    }
}
