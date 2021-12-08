using MediatR;
using OzonEdu.MerchandiseService.Domain.AggregationModels.EmployeeAggregate;


namespace OzonEdu.MerchandiseService.Infrastructure.Commands
{
    public class EmployeeDetectedCommand : IRequest<Employee>
    {
        public Employee TheEmployee { get; set; }

        public long EmployeeId { get; set; }

        //public EmployeeDetectedCommand(Employee employee)
        //{
        //    TheEmployee = employee;
        //    EmployeeId = 0;
        //}

        //public EmployeeDetectedCommand(long employeeId)
        //{
        //    EmployeeId = employeeId;
        //    TheEmployee = null;
        //}
    }
}
