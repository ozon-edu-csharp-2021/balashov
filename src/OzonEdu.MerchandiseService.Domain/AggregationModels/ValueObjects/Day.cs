using System;
using System.Collections.Generic;
using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects
{
    public class Day : ValueObject
    {
        public int TheDay { get; }

        public Day(int day)
        {
            if (DayValidation(day))
                TheDay = day;
            else
                throw new Exception("Day is not valid");
        }
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return TheDay;
        }

        private static bool DayValidation(int day)
        {
            if (day < 1) return false;
            if (day > 31) return false;

            return true;
        }
    }
}