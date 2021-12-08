﻿using System;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using OzonEdu.MerchandiseService.Infrastructure.Configuration;

namespace OzonEdu.MerchandiseService.Infrastructure.MessageBroker
{
    public class ProducerBuilderWrapper : IProducerBuilderWrapper
    {
        /// <inheritdoc cref="Producer"/>
        public IProducer<string, string> Producer { get; set; }

        /// <inheritdoc cref="EmailNotificationTopic"/>
        public string EmailNotificationTopic { get; set; }

        public ProducerBuilderWrapper(IOptions<KafkaConfiguration> configuration)
        {
            var configValue = configuration.Value;
            if (configValue is null)
                throw new ApplicationException("Configuration for kafka server was not specified");

            var producerConfig = new ProducerConfig
            {
                BootstrapServers = configValue.BootstrapServers
            };

            Producer = new ProducerBuilder<string, string>(producerConfig).Build();
            EmailNotificationTopic = configValue.EmailTopic;
        }
    }
}
