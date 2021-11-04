
using System.Collections.Generic;
using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects
{
    public class PhoneNumber : ValueObject
    {
        public PhoneNumber(string phone)
            => Phone = phone;

        public string Phone { get; }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Phone;
        }
    }
}
