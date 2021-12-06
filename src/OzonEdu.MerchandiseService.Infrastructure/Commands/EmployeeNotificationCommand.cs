using MediatR;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate;
using OzonEdu.MerchandiseService.Infrastructure.ExternalEvents;

namespace OzonEdu.MerchandiseService.Infrastructure.Commands
{
    public class EmployeeNotificationCommand : IRequest<MerchandiseRequest>
    {
        public FullNotificationEvent EmployeeNotificationEvent { get; set; }
    }
}
