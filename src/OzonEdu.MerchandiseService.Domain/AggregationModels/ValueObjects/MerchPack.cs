using System.Collections.Generic;
using OzonEdu.MerchandiseService.Domain.AggregationModels.Enumerations;
using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects
{
    public class MerchPack : ValueObject
    {
        public MerchLine Line { get;  }

        public MerchPackTitle PackTitle { get; }

        public MerchPack(MerchLine line, MerchPackTitle packTitle)
        {
            Line = line;
            PackTitle = packTitle;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Line;
            yield return PackTitle;
        }

        public override string ToString()
        {
            return $"Тип набора: {PackTitle.Name}; Серия: {Line.LineName}, {Line.Year} [{Line.MerchLineTag}]";
        }
    }
}
