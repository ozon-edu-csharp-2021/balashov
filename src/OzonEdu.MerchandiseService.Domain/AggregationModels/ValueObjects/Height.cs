using System.Collections.Generic;
using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects
{
    public class HeightMetric : ValueObject
    {
        public double Centimeters { get; }

        private HeightMetric(double centimeters) => Centimeters = centimeters;

        public static HeightMetric FromMetrics(double centimeters, out string exceptionString)
        {
            exceptionString = string.Empty;

            if (centimeters <= 70 )
            {
                exceptionString = "Height is too small (< 70cm or ~2feet)";
                return null;
            }

            if (centimeters > 250)
            {
                exceptionString = "Height is too big (> 250cm or ~9 feet)";
                return null;
            }

            return new HeightMetric(centimeters);
        }

        public static HeightMetric FromImperial(int feet, int inches, out string exceptionString)
        {
            var centimeters = (feet * 12 + inches) * 2.54;
           
            return FromMetrics(centimeters, out exceptionString);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Centimeters;
        }
    }
}
