using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate;

namespace OzonEdu.MerchandiseService.Infrastructure.InterfacesToExternals
{
    public interface IStockApiServer
    {
        public bool IsMerchAmountEnough(MerchandiseRequest merchRequest);

        public bool ReserveMerch(MerchandiseRequest merchRequest);
    }
}
