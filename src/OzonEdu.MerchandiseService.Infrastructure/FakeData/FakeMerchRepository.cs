using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate;
using OzonEdu.MerchandiseService.Domain.Contracts;

namespace OzonEdu.MerchandiseService.Infrastructure.FakeData
{
    class FakeMerchRepository : IMerchRepository
    {
        public IUnitOfWork UnitOfWork { get; }

        public Task<MerchandiseRequest> CreateAsync(MerchandiseRequest itemToCreate, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<MerchandiseRequest> UpdateAsync(MerchandiseRequest itemToUpdate, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<MerchandiseRequest> FindByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<MerchandiseRequest>> FindByEmployeeIdAsync(long employeeId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
