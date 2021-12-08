using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects
{
    public class Email : ValueObject
    {
        public Email(string emailString)
        {
            if (EmailValidationViaRegex(emailString))
                EmailString = emailString;
            else
                throw new Exception("Email is not valid");
        }

        public string EmailString { get; }

        public static bool EmailValidationViaRegex(string emailString)
        {
            string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                             + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                             + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            var regex = new Regex(pattern, RegexOptions.IgnoreCase);

            return regex.IsMatch(emailString);
        }

        public override string ToString()
        {
            return EmailString;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return EmailString;
        }
    }
}
