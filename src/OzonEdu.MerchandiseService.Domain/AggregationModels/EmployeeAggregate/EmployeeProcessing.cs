using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.EmployeeAggregate
{
    public class EmployeeProcessing
    {
        //todo Возможная бизнес-логика: если работника нет в базе, то сформировать его из переданых данных, записать и вернуть

        public static async Task<Employee> GetByIdAsync(IEmployeeRepository employeeRepository, long employeeId, CancellationToken cancellationToken)
        {
            if (employeeId > 0)
            {
                var employee = await employeeRepository.FindByIdAsync(employeeId, cancellationToken);
                return employee;
            }
            else
            {
                return null;
            }
        }
    }
}
