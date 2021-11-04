using System.Collections.Generic;
using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects
{
    public class Day : ValueObject
    {
        public int TheDay { get; }

        public Day(int day)
        {
            TheDay = day;
        }
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return TheDay;
        }
    }
}