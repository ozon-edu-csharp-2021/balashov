using OzonEdu.MerchandiseService.Domain.AggregationModels.Enumerations;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate
{
    class Merch : Entity
    {
        public SKU Sku { get; }

        public Title MerchTitle { get; }

        public MerchLine MerchLine { get; }

        public MerchType MerchType { get; }

        public Size Size { get; }

        public Merch(SKU sku, Title merchTitle, MerchLine merchLine, MerchType merchType, Size size)
        {
            Sku = sku;

            MerchTitle = merchTitle;

            MerchLine = merchLine;

            MerchType = merchType;

            Size = size;
        }

        public void SetId(int id)
        {
            Id = id;
        }
    }
}
