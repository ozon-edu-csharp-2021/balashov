using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OzonEdu.MerchandiseService.Domain.Events;

namespace OzonEdu.MerchandiseService.Infrastructure.Handlers.DomainEvents
{
    public class MerchRequestDoneHandler : INotificationHandler<MerchRequestDoneDomainEvent>
    {

        public MerchRequestDoneHandler()
        {
        }

        public async Task Handle(MerchRequestDoneDomainEvent notification, CancellationToken cancellationToken)
        {

        }
    }
}
