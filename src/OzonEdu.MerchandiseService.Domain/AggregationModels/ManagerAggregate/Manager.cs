using OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.ManagerAggregate
{
    public class Manager : Entity
    {
        private const int maxTasksCount = 3;
        public Manager(PersonName name, Email email, PhoneNumber phone,int assignedTasks)
        {
            Name = name;
            Email = email;
            PhoneNumber = phone;
            AssignedTasks = assignedTasks;
        }

        public PersonName Name { get; }

        public PhoneNumber PhoneNumber { get; }
        
        public Email Email { get; }

        public int AssignedTasks { get; private set; }

        public bool CanHandleNewTask => AssignedTasks < maxTasksCount;

        public void AssignTask() => AssignedTasks++;
    }
}
