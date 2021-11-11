using System;
using System.Collections.Generic;
using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects
{
    public class Month : ValueObject
    {
        public int TheMonth { get; }

        public Month(int month)
        {
            if (MonthValidation(month))
                TheMonth = month;
            else
                throw new Exception("Month is not valid");
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return TheMonth;
        }

        private static bool MonthValidation(int month)
        {
            if (month < 1) return false;
            if (month > 12) return false;

            return true;
        }
    }
}