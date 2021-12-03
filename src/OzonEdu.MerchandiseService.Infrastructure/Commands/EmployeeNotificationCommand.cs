using CSharpCourse.Core.Lib.Events;
using MediatR;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate;

namespace OzonEdu.MerchandiseService.Infrastructure.Commands
{
    public class EmployeeNotificationCommand : IRequest<MerchandiseRequest>
    {
        public NotificationEvent EmployeeNotificationEvent { get; set; }
    }
}
