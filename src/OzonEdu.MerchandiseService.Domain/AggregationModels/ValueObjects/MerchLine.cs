using System.Collections.Generic;
using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects
{
    public class MerchLine : ValueObject
    {
        public string LineName { get; }

        public int Year { get; }

        public Tag MerchLineTag { get; }
        
        public MerchLine(string lineName, int year)
        {
            LineName = lineName;
            Year = year;
        }

        public MerchLine(string lineName, int year, Tag merchLineTag)
        {
            LineName = lineName;
            Year = year;
            MerchLineTag = merchLineTag;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return LineName;
            yield return Year;
            yield return MerchLineTag;
        }

        public override string ToString()
        {
            return $"{LineName}, {Year} {MerchLineTag}";
        }
    }
}
