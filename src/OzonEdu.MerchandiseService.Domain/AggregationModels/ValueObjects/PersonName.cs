using System.Collections.Generic;
using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects
{
    public class PersonName : ValueObject
    {
        public string FirstName { get;}
        public string LastName { get;}
        public string MiddleName { get; }

        private PersonName(string firstName, string lastName, string middleName)
        {
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
        }

        public static PersonName CreateFromOneString(string fullName)
        {
            var names = fullName.Split(" ");

            if (names.Length == 0)
                return null;

            var firstName = names[0];

            var lastName = names.Length >= 2? names[1] : "";
            
            var middleName = names.Length == 3? names[2] : "";

            return Create(firstName, lastName, middleName);
        }

        public static PersonName Create(string firstName, string lastName)
        {
            return new PersonName(firstName, lastName, "");
        }

        public static PersonName Create(string firstName, string lastName, string middleName)
        {
            return new PersonName(firstName, lastName, middleName);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return FirstName;
            yield return LastName;
        }

        public override string ToString()
        {
            return $"{LastName} {FirstName} {MiddleName}";
        }
    }
}
