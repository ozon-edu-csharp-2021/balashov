using System;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate;

namespace OzonEdu.MerchandiseService.Infrastructure.InterfacesToExternals
{
    public class StockApiServer : IStockApiServer
    {
        public bool IsMerchAmountEnough(MerchandiseRequest merchRequest)
        {
            throw new NotImplementedException();
        }

        public bool ReserveMerch(MerchandiseRequest merchRequest)
        {
            throw new NotImplementedException();
            //TODO реализовать обращение на склад через GRPC
        }
    }
}
