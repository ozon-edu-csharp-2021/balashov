using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchandiseService.Domain.Events;
using OzonEdu.MerchandiseService.Infrastructure.ExternalDataSources;

namespace OzonEdu.MerchandiseService.Infrastructure.Handlers.DomainEvents
{
    public class MerchRequestAssignedHandler : INotificationHandler<MerchRequestAssignedDomainEvent>
    {
        private readonly IStockApiServer _stockApiServer;
        private readonly IMerchRepository _merchRepository;


        public MerchRequestAssignedHandler(IStockApiServer stockApiServer, IMerchRepository merchRepository)
        {
            _stockApiServer = stockApiServer;
            _merchRepository = merchRepository;
        }

        public async Task Handle(MerchRequestAssignedDomainEvent notification, CancellationToken cancellationToken)
        {
            var merchRequest = notification.MerchRequest;
            var date = new Date(DateTime.Now);

            //Проверяется наличие/зарезервировать мерч на складе через запрос к stock - api
            //Если все проверки прошли - резервируется мерч в stock - api
            if (_stockApiServer.ReserveMerch(merchRequest))
            {
                if (!merchRequest.SetReserved(date))
                    throw new Exception($"Заявка id:{merchRequest.Id} не назначена в резерв! Метод вернул отказ");
                    
                await _merchRepository.UpdateAsync(merchRequest, cancellationToken);
            }
            else
            {
                //todo можно добавить в MerchandiseRequest поле Comment и писать туда явно и подробно что ожидается поставка на склад
            }
        }
    }
}
