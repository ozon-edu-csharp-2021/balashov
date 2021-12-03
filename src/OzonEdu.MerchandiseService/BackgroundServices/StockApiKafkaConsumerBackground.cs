using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using CSharpCourse.Core.Lib.Events;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OzonEdu.MerchandiseService.Infrastructure.Commands;
using OzonEdu.MerchandiseService.Infrastructure.MessageBroker;

namespace OzonEdu.MerchandiseService.BackgroundServices
{
    public class StockApiKafkaConsumerBackground : BackgroundService
    {
        private readonly string _topicName;
        private readonly IConsumerBuilderWrapper _consumerBuilderWrapper;
        
        private readonly IMediator _mediator;
        private readonly ILogger<StockApiKafkaConsumerBackground> _logger;

        public StockApiKafkaConsumerBackground(
            ILogger<StockApiKafkaConsumerBackground> logger,
            IConsumerBuilderWrapper consumerBuilderWrapper, 
            IMediator mediator)
        {
            _logger = logger;
            _consumerBuilderWrapper = consumerBuilderWrapper;
            _mediator = mediator;

            _topicName = _consumerBuilderWrapper.StockReplenishedEvent;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var kafkaConsumerTask = Task.Run(
                async () =>
                {
                    var consumer = _consumerBuilderWrapper.Consumer;

                    consumer.Subscribe(_topicName);

                    await DoConsumeWhileAsync(stoppingToken, consumer);
                },
                stoppingToken);

            _consumerBuilderWrapper.Consumer.Unsubscribe();

            return kafkaConsumerTask;
        }

        private async Task DoConsumeWhileAsync(CancellationToken stoppingToken, IConsumer<long, string> consumer)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    await DoConsumeAsync(stoppingToken, consumer);
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Kafka-consume closing. Topic: {topicName}", _topicName);
                _consumerBuilderWrapper.Consumer.Unsubscribe();
            }
        }

        private async Task DoConsumeAsync(CancellationToken stoppingToken, IConsumer<long, string> consumer)
        {
            try
            {
                var cr = consumer.Consume(stoppingToken);
                var stockReplenishedMessage = JsonConvert.DeserializeObject<StockReplenishedEvent>(cr.Message.Value);
                _logger.LogInformation("Kafka income StockReplenishedEvent message.");

                if (stockReplenishedMessage != null)
                {
                    var mediatrRequest = new StockReplenishedCommand { StockReplenishedItems = stockReplenishedMessage.Type };

                    await _mediator.Send(mediatrRequest, stoppingToken);
                }
            }
            catch (Exception e)
            {
                if (e is OperationCanceledException)
                    throw;
                _logger.LogError(e, "Kafka-consume error: {KafkaError}");
            }
        }
    }
}


