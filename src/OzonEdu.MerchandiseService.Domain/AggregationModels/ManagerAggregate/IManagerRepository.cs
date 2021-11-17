using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchandiseService.Domain.Contracts;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.ManagerAggregate
{
    public interface IManagerRepository : IRepository<Manager>
    {
        Task<Manager> FindByIdAsync(long id, CancellationToken cancellationToken = default);

        Task<List<Manager>> FindByNameIdAsync(PersonName personName, CancellationToken cancellationToken = default);
        Task<List<Manager>> GetAll(CancellationToken cancellationToken = default);
    }
}
