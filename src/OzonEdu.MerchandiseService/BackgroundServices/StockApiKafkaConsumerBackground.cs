using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using CSharpCourse.Core.Lib.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
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

        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<StockApiKafkaConsumerBackground> _logger;

        public StockApiKafkaConsumerBackground(
            ILogger<StockApiKafkaConsumerBackground> logger,
            IConsumerBuilderWrapper consumerBuilderWrapper, 
            IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _consumerBuilderWrapper = consumerBuilderWrapper;
            _serviceScopeFactory = serviceScopeFactory;

            _topicName = _consumerBuilderWrapper.StockReplenishedEvent;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var kafkaConsumerTask = Task.Run(
                async () =>
                {
                    var consumer = _consumerBuilderWrapper.ConsumerStock;

                    consumer.Subscribe(_topicName);

                    await DoConsumeWhileAsync(stoppingToken, consumer);
                },
                stoppingToken);

            _consumerBuilderWrapper.ConsumerStock.Unsubscribe();

            return kafkaConsumerTask;
        }

        private async Task DoConsumeWhileAsync(CancellationToken stoppingToken, IConsumer<string, string> consumer)
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
                _consumerBuilderWrapper.ConsumerStock.Unsubscribe();
            }
        }

        private async Task DoConsumeAsync(CancellationToken stoppingToken, IConsumer<string, string> consumer)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                var cr = consumer.Consume(stoppingToken);
                var stockReplenishedMessage = JsonConvert.DeserializeObject<StockReplenishedEvent>(cr.Message.Value);
                _logger.LogInformation("Kafka income StockReplenishedEvent message.");

                if (stockReplenishedMessage != null)
                {
                    var mediatrRequest = new StockReplenishedCommand { StockReplenishedItems = stockReplenishedMessage.Type };

                    await mediator.Send(mediatrRequest, stoppingToken);
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


