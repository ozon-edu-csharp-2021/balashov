using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects
{
    public class PhoneNumber : ValueObject
    {
        public string Phone { get; }

        public PhoneNumber(string phone)
        {
            if (PhoneNumberValidation(phone))
                Phone = phone;
            else
                throw new Exception("Phone number is not valid");
        }
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Phone;
        }

        private static bool PhoneNumberValidation(string phone)
        {
            return Regex.Match(phone, @"^(\+[0-9]{9})$").Success;
        }
    }
}
