using System.Collections.Generic;
using OzonEdu.MerchandiseService.Domain.AggregationModels.Enumerations;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate
{
    public class MerchPack : ValueObject
    {
        public MerchLine Line { get; set; }

        public MerchPackTitle PackTitle { get; set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Line;
            yield return PackTitle;
        }
    }
}
