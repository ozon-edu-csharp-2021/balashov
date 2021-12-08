using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OzonEdu.MerchandiseService.Domain.Events;

namespace OzonEdu.MerchandiseService.Infrastructure.Handlers.DomainEvents
{
    public class MerchRequestCreatedHandler : INotificationHandler<MerchRequestCreatedDomainEvent>
    {
        public Task Handle(MerchRequestCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
           return Task.CompletedTask;
        }
    }
}
