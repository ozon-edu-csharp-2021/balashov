using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseService.Domain.AggregationModels.EmployeeAggregate;

namespace OzonEdu.MerchandiseService.Infrastructure.ExternalDataSources
{
    public interface IEmployeeServer
    {
        Task<Employee> GetByIdAsync(long employeeId, CancellationToken cancellationToken);

        Task<List<Employee>> GetAll(CancellationToken cancellationToken);
    }
}
