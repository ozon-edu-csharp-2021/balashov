using Confluent.Kafka;

namespace OzonEdu.MerchandiseService.Infrastructure.MessageBroker
{
    public interface IProducerBuilderWrapper
    {
        /// <summary>
        /// Producer instance
        /// </summary>
        IProducer<long, string> Producer { get; set; }

        /// <summary>
        /// Топик для отправки сообщения что пришла новая поставка
        /// </summary>
        string EmailNotificationTopic { get; set; }
    }
}
