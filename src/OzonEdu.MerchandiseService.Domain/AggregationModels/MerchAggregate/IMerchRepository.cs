using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseService.Domain.Contracts;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate
{
    public interface IMerchRepository : IRepository<MerchandizeRequest>
    {
        Task<MerchandizeRequest> FindByIdAsync(long id, CancellationToken cancellationToken = default);

        Task<List<MerchandizeRequest>> FindByPersonIdAsync(long personId, CancellationToken cancellationToken = default);
    }
}
