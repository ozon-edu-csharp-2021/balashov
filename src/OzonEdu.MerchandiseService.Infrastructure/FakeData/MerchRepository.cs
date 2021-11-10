using System;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchandiseService.Domain.Contracts;

namespace OzonEdu.MerchandiseService.Infrastructure.FakeData
{
    class MerchRepository : IMerchRepository
    {
        public IUnitOfWork UnitOfWork { get; }

        public Task<MerchandizeRequest> CreateAsync(MerchandizeRequest itemToCreate, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<MerchandizeRequest> UpdateAsync(MerchandizeRequest itemToUpdate, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<MerchandizeRequest> FindByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<MerchandizeRequest> FindBySkuAsync(SKU sku, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
