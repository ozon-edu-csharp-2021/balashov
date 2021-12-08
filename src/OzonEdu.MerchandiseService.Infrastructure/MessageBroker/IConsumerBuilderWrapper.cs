using Confluent.Kafka;

namespace OzonEdu.MerchandiseService.Infrastructure.MessageBroker
{
    public interface IConsumerBuilderWrapper
    {
        IConsumer<string, string> ConsumerEmployee { get; set; }

        IConsumer<string, string> ConsumerStock { get; set; }

        string StockReplenishedEvent { get; set; }

        string EmployeeNotificationEvent { get; set; }

    }
}
