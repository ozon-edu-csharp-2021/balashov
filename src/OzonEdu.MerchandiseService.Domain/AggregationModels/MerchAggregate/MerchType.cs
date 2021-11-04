using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate
{
    public class MerchType : Enumeration
    {
        public static MerchType TShirt = new(1, nameof(TShirt));
        public static MerchType Sweatshirt = new(2, nameof(Sweatshirt));
        public static MerchType Cup = new(3, nameof(Cup));
        public static MerchType Notepad = new(4, nameof(Notepad));
        public static MerchType Bag = new(5, nameof(Bag));
        public static MerchType Pen = new(6, nameof(Pen));
        public static MerchType Socks = new(7, nameof(Socks));

        public MerchType(int id, string name) : base(id, name)
        {
        }
    }
}