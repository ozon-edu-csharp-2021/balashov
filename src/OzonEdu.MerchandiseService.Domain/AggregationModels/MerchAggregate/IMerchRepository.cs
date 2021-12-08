using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseService.Domain.Contracts;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate
{
    public interface IMerchRepository : IRepository<MerchandiseRequest>
    {
        Task<MerchandiseRequest> FindByIdAsync(long id, CancellationToken cancellationToken = default);

        Task<List<MerchandiseRequest>> FindByEmployeeEmailAsync(string employeeEmail, CancellationToken cancellationToken = default);
    }
}
