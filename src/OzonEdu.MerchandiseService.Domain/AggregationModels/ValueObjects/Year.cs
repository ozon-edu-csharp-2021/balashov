using System.Collections.Generic;
using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects
{
    public class Year : ValueObject
    {
        public int TheYear { get; }

        public Year(int year)
        {
            TheYear = year;
        }
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return TheYear;
        }
    }
}