using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchandiseService.Domain.Contracts;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate
{
    public interface IMerchRepository : IRepository<MerchPack>
    {
        Task<MerchPack> FindByIdAsync(long id, CancellationToken cancellationToken = default);

        Task<MerchPack> FindBySkuAsync(SKU sku, CancellationToken cancellationToken = default);
    }
}
