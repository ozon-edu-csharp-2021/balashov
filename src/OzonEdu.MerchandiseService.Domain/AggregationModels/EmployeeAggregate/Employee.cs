using OzonEdu.MerchandiseService.Domain.AggregationModels.Enumerations;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.EmployeeAggregate
{
    public class Employee : Entity
    {
        public Employee(PersonName name, Email email, PhoneNumber phoneNumber)
        {
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
        }

        public PersonName Name { get; }

        public PhoneNumber PhoneNumber { get; }

        public Email Email { get; private set; }

        public Employee SetId(long id)
        {
            Id = id;
            return this;
        }
    }
}
