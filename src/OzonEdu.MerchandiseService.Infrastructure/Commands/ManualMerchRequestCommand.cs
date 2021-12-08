using MediatR;
using OzonEdu.MerchandiseService.Domain.AggregationModels.Enumerations;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects;

namespace OzonEdu.MerchandiseService.Infrastructure.Commands
{
    public class ManualMerchRequestCommand : IRequest<MerchandiseRequest>
    {
        public long HRManagerId { get; set; }

        public long EmployeeId { get; set; }

        public MerchPack RequestedMerchPack { get; set; }

        public Size Size { get; set; }

        public Date Date { get; set; }
    }
}