using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseService.Domain.AggregationModels.EmployeeAggregate;

namespace OzonEdu.MerchandiseService.Infrastructure.InterfacesToExternals.FakeExternals
{
    public class FakeEmployeeServer : IEmployeeServer
    {
        private readonly IEmployeeRepository _employeeRepository;

        public FakeEmployeeServer(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<Employee> GetByIdAsync(long employeeId, CancellationToken cancellationToken)
        {
            if (employeeId > 0)
            {
                var employee = await _employeeRepository.FindByIdAsync(employeeId, cancellationToken);
                return employee;
            }
            else
            {
                return null;
            }
        }

        public Task<List<Employee>> GetAll(CancellationToken cancellationToken)
        {
            return _employeeRepository.GetAllAsync(cancellationToken);
        }
    }
}
