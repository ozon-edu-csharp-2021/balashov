using System.Text.Json;
using System.Threading.Tasks;
using Confluent.Kafka;
using CSharpCourse.Core.Lib.Enums;
using CSharpCourse.Core.Lib.Events;
using OzonEdu.MerchandiseService.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ManagerAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate;
using OzonEdu.MerchandiseService.Infrastructure.MessageBroker;

namespace OzonEdu.MerchandiseService.Infrastructure.ExternalDataSources
{
    public class EmailServer : IEmailServer
    {
        private readonly IProducerBuilderWrapper _producerBuilderWrapper;

        public EmailServer(IProducerBuilderWrapper producerBuilderWrapper)
        {
            _producerBuilderWrapper = producerBuilderWrapper;
        }

        public Task SendEmailAboutMerchAsync(Employee employee, Manager manager, MerchandiseRequest merchRequest)
        {
            //TODO проверить формат и работу ключа в сообщении
            _producerBuilderWrapper.Producer.Produce(_producerBuilderWrapper.EmailNotificationTopic,
                new Message<long, string>()
                {
                    Key = employee.Id,
                    Value = JsonSerializer.Serialize(new NotificationEvent()
                    {
                        EmployeeEmail = employee.Email.EmailString,
                        EmployeeName = employee.Name.ToString(),
                        EventType = EmployeeEventType.MerchDelivery,
                        ManagerEmail = manager.Email.EmailString,
                        ManagerName = manager.Name.ToString(),
                        Payload = merchRequest.RequestedMerchPack.PackTitle.Id
                    })
                });

            return Task.CompletedTask;
        }
    }
}
