using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseService.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ManagerAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchandiseService.Domain.Contracts;

namespace OzonEdu.MerchandiseService.Infrastructure.FakeData
{
    class FakeManagerRepository : IManagerRepository
    {
        public IUnitOfWork UnitOfWork { get; }
        public Task<Manager> CreateAsync(Manager itemToCreate, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Manager> UpdateAsync(Manager itemToUpdate, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Manager> FindByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<Manager>> FindByNameIdAsync(PersonName personName, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<Manager>> GetAll(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
