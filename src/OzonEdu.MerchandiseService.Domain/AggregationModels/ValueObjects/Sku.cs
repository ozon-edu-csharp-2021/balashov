using System.Collections.Generic;
using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects
{
    public class SKU : ValueObject
    {
        public long TheSku { get; }
        
        public SKU(long sku)
        {
            TheSku = sku;
        }
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return TheSku;
        }
    }
}