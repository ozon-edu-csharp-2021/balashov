using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate;
using OzonEdu.MerchandiseService.Infrastructure.Commands;

namespace OzonEdu.MerchandiseService.Infrastructure.Handlers
{
    public class StockReplenishedCommandHandler : IRequestHandler<StockReplenishedCommand, MerchandiseRequest>
    {
        private readonly ILogger<StockReplenishedCommandHandler> _logger;

        public StockReplenishedCommandHandler(ILogger<StockReplenishedCommandHandler> logger)
        {
            _logger = logger;
        }

        public Task<MerchandiseRequest> Handle(StockReplenishedCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Получено сообщение из StockApi про новую поставку");
            
            return null;
        }
    }
}
