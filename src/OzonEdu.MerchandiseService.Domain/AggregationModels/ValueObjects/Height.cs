using System;
using System.Collections.Generic;
using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects
{
    public class HeightMetric : ValueObject
    {
        public int Centimeters { get; }

        private HeightMetric(int centimeters) => Centimeters = centimeters;

        public static HeightMetric FromMetrics(int centimeters)
        {
            if (centimeters < 70 )
            {
                throw new Exception("Height is too small (< 70cm or ~2feet)");
            }

            if (centimeters > 250)
            {
                throw new Exception("Height is too big (> 250cm or ~9 feet)");
            }

            return new HeightMetric(centimeters);
        }

        public static HeightMetric FromImperial(int feet, int inches)
        {
            if(feet< 0 || inches < 0)
                throw new Exception("Incorrect Height parameters");

            var centimeters = (int) Math.Round((feet * 12 + inches) * 2.54);
           
            return FromMetrics(centimeters);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Centimeters;
        }
    }
}
