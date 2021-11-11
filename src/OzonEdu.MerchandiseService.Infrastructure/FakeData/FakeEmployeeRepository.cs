using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseService.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchandiseService.Domain.Contracts;

namespace OzonEdu.MerchandiseService.Infrastructure.FakeData
{
    class FakeEmployeeRepository : IEmployeeRepository
    {
        public IUnitOfWork UnitOfWork { get; }
        public Task<Employee> CreateAsync(Employee itemToCreate, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Employee> UpdateAsync(Employee itemToUpdate, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Employee> FindByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<Employee>> FindByNameIdAsync(PersonName personName, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
