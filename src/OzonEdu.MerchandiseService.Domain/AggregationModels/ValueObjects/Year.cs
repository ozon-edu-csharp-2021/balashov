using System;
using System.Collections.Generic;
using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects
{
    public class Year : ValueObject
    {
        public int TheYear { get; }

        public Year(int year)
        {
            if (YearValidation(year))
                TheYear = year;
            else
                throw new Exception("Year is not valid");
        }
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return TheYear;
        }

        private static bool YearValidation(int year)
        {
            if (year < 2020) return false;
            if (year > 2100) return false;

            return true;
        }
    }
}