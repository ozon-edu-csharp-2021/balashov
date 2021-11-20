using OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.ManagerAggregate
{
    public class Manager : Entity
    {
        public const int MaxTasksCount = 3;

        public Manager(PersonName name, Email email, PhoneNumber phone)
        {
            Name = name;
            Email = email;
            PhoneNumber = phone;
        }

        public Manager(PersonName name, Email email, PhoneNumber phone, int assignedTasks)
            :this(name, email, phone)
        {
            AssignedTasks = assignedTasks;
        }

        public PersonName Name { get; }

        public PhoneNumber PhoneNumber { get; }
        
        public Email Email { get; }

        public int AssignedTasks { get; private set; }

        public bool CanHandleNewTask => AssignedTasks < MaxTasksCount;

        public void AssignTask() => AssignedTasks++;

        public Manager SetId(int id)
        {
            Id = id;
            return this;
        }
    }
}
