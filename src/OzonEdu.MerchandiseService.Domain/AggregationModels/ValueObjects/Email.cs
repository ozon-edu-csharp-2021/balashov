
using System.Collections.Generic;
using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects
{
    public class Email : ValueObject
    {
        public Email(string emailString)
            => EmailString = emailString;

        public string EmailString { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return EmailString;
        }
    }
}
