using System;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using OzonEdu.MerchandiseService.Infrastructure.Configuration;

namespace OzonEdu.MerchandiseService.Infrastructure.MessageBroker
{
    public class ConsumerBuilderWrapper : IConsumerBuilderWrapper
    {
        /// <inheritdoc cref="ConsumerEmployee"/>
        public IConsumer<string, string> ConsumerEmployee { get; set; }

        /// <inheritdoc cref="ConsumerStock"/>
        public IConsumer<string, string> ConsumerStock { get; set; }

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

            ConsumerEmployee = new ConsumerBuilder<string, string>(consumerConfig).Build();
            EmployeeNotificationEvent = configValue.EmployeeNotificationTopic;

            ConsumerStock = new ConsumerBuilder<string, string>(consumerConfig).Build();
            StockReplenishedEvent = configValue.StockTopic;
        }
    }
}
