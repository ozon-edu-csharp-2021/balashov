using System.Collections.Generic;
using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects
{
    public class Month : ValueObject
    {
        public int TheMonth { get; }

        public Month(int month)
        {
            TheMonth = month;
        }
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return TheMonth;
        }
    }
}