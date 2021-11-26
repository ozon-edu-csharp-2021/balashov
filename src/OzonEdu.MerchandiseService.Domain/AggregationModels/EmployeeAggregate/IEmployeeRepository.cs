using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchandiseService.Domain.Contracts;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.EmployeeAggregate
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<Employee> FindByIdAsync(long id, CancellationToken cancellationToken = default);

        Task<List<Employee>> FindByNameIdAsync(PersonName personName, CancellationToken cancellationToken = default);

        Task<List<Employee>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
