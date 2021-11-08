using MediatR;

namespace OzonEdu.MerchandiseServise.Infrastructure.Commands
{
    public class BDSCommand : IRequest
    {
        public long Sku { get; set; }
    }
}