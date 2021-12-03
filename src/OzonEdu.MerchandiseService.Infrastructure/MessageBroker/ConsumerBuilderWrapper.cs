using System;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using OzonEdu.MerchandiseService.Infrastructure.Configuration;

namespace OzonEdu.MerchandiseService.Infrastructure.MessageBroker
{
    public class ConsumerBuilderWrapper : IConsumerBuilderWrapper
    {
        /// <inheritdoc cref="Consumer"/>
        public IConsumer<long, string> Consumer { get; set; }

        /// <inheritdoc cref="StockReplenishedEvent"/>
        public string StockReplenishedEvent { get; set; }

        /// <inheritdoc cref="EmployeeNotificationEvent"/>
        public string EmployeeNotificationEvent { get; set; }

        public ConsumerBuilderWrapper(IOptions<KafkaConfiguration> configuration)
        {
            var configValue = configuration.Value;
            if (configValue is null)
                throw new ApplicationException("Configuration for kafka server was not specified");

            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = configValue.BootstrapServers,
                GroupId = configuration.Value.GroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };

            Consumer = new ConsumerBuilder<long, string>(consumerConfig).Build();
            
            StockReplenishedEvent = configValue.StockTopic;
            EmployeeNotificationEvent = configValue.EmployeeNotificationTopic;
        }
    }
}
