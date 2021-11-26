using MediatR;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate;

namespace OzonEdu.MerchandiseService.Domain.Events
{

    public class MerchRequestAssignedDomainEvent : INotification
    {
        public MerchandiseRequest MerchRequest { get; }

        public MerchRequestAssignedDomainEvent(MerchandiseRequest merchRequest)
        {
            MerchRequest = merchRequest;
        }
    }
}
