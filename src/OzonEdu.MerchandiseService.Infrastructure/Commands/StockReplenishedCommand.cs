using System.Collections.Generic;
using CSharpCourse.Core.Lib.Models;
using MediatR;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate;

namespace OzonEdu.MerchandiseService.Infrastructure.Commands
{
    public class StockReplenishedCommand : IRequest<MerchandiseRequest>
    {
        public IReadOnlyCollection<StockReplenishedItem> StockReplenishedItems { get; set; }
    }
}
