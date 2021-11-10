using MediatR;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate;
using OzonEdu.MerchandiseService.Infrastructure.Commands;

namespace OzonEdu.MerchandiseService.Infrastructure.Handlers
{
    class BDSHandler : IRequestHandler<BDSCommand>
    {
        private readonly IMerchRepository _merchRepository;

        public BDSHandler(IMerchRepository merchRepository)
        {
            _merchRepository = merchRepository;
        }

        public Task<Unit> Handle(BDSCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
