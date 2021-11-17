using OzonEdu.MerchandiseService.Domain.AggregationModels.Enumerations;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.EmployeeAggregate
{
    public class Employee : Entity
    {
        public Employee(PersonName name, Email email, PhoneNumber phoneNumber, Size size, HeightMetric height)
        {
            Name = name;
            Email = email;
            Size = size;
            Height = height;
            PhoneNumber = phoneNumber;
        }

        public PersonName Name { get; }

        public PhoneNumber PhoneNumber { get; }

        public Email Email { get; private set; }

        public Size Size { get; }
        
        public HeightMetric Height { get; }

        public void SetId(int id)
        {
            Id = id;
        }
    }
}
