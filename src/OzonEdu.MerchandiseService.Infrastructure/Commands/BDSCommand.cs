using MediatR;

namespace OzonEdu.MerchandiseService.Infrastructure.Commands
{
    public class BDSCommand : IRequest
    {
        public long Sku { get; set; }
    }
}