using Confluent.Kafka;

namespace OzonEdu.MerchandiseService.Infrastructure.MessageBroker
{
    public interface IConsumerBuilderWrapper
    {
        IConsumer<long, string> Consumer { get; set; }

        string StockReplenishedEvent { get; set; }

        public string EmployeeNotificationEvent { get; set; }

    }
}
