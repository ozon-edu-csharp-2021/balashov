using OzonEdu.MerchandiseService.Domain.AggregationModels.Enumerations;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate
{
    class Merch : Entity
    {
        public SKU Sku { get; set; }

        public Title MerchTitle { get; set; }

        public MerchLine MerchLine { get; set; }

        public MerchType MerchType { get; set; }

        public Size Size { get; set; }

    }
}
