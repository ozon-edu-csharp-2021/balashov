using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseService.Domain.Contracts;

namespace OzonEdu.MerchandiseService.Infrastructure.FakeData
{
    class FakeUnitOfWork : IUnitOfWork
    {
        public ValueTask StartTransaction(CancellationToken token)
        {
            return default;
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            return null;
        }


        public void Dispose()
        {
        }
    }
}
