using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace OzonEdu.MerchandiseServise.Infrastructure.Handlers.DomainEvent
{
    public class ReachedMinimumDomainEventHandler : INotificationHandler<ReachedMinimumStockItemsNumberDomainEvent> 
    {
        public Task Handle(ReachedMinimumStockItemsNumberDomainEvent notification, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}