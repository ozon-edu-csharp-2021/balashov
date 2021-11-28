using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OzonEdu.MerchandiseService.Domain.AggregationModels.Enumerations;
using OzonEdu.MerchandiseService.Domain.Events;
using OzonEdu.MerchandiseService.Infrastructure.InterfacesToExternals;

namespace OzonEdu.MerchandiseService.Infrastructure.Handlers.DomainEvents
{
    public class MerchReservedOnStockHandler : INotificationHandler<MerchReservedOnStockDomainEvent>
    {
        private readonly IEmailServer _emailServer;

        public MerchReservedOnStockHandler(IEmailServer emailServer)
        {
            _emailServer = emailServer;
        }

        public async Task Handle(MerchReservedOnStockDomainEvent notification, CancellationToken cancellationToken)
        {
            //Выслать е-mail если резерв произведён и ожидается получение мерча работником
            var merchRequest = notification.MerchRequest;
            
            if (!merchRequest.Status.Status.Equals(MerchRequestStatusType.Reserved))
                return;

            var text = $"Набор мерча зарезервирован и ожидает тебя!\nВот такой набор: {merchRequest.RequestedMerchPack}\nЗабери его скорее :)";
            await _emailServer.SendEmailAboutMerchAsync(notification.MerchRequest.EmployeeId, text);

        }
    }
}
