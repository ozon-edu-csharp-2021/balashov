using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace OzonEdu.MerchandiseServise.Infrastructure.Handlers.DeliveryRequestAggregate
{
    public class CreateDeliveryRequestCommandHandler : IRequestHandler<CreateDeliveryRequestCommand>
    {
        public readonly IDeliveryRequestRepository _deliveryRequestRepository;
        
        public CreateDeliveryRequestCommandHandler(IDeliveryRequestRepository deliveryRequestRepository)
        {
            _deliveryRequestRepository = deliveryRequestRepository ?? 
                                         throw new ArgumentNullException($"{nameof(deliveryRequestRepository)}");
        }
        
        public async Task<Unit> Handle(CreateDeliveryRequestCommand request, CancellationToken cancellationToken)
        {
            var deliveryRequest = new DeliveryRequest(
                null,
                RequestStatus.InWork,
                request.SkuCollection.Select(it => new Sku(it)).ToList());
            
            //TODO Тут должен быть запрос к сервису поставок для получения идентификатора поставки
            // и этот идентификатор нужно будет проставить в модель

            await _deliveryRequestRepository.CreateAsync(deliveryRequest, cancellationToken);
            await _deliveryRequestRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}