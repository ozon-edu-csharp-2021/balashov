using System;
using System.Collections.Generic;
using System.Linq;
using OzonEdu.MerchandiseService.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ManagerAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects;

namespace OzonEdu.MerchandiseService.Domain.DomainServices
{
    public sealed class MerchDomainService
    {
        public static MerchandizeRequest CreateMerchandizeRequest(IEnumerable<Manager> managers, Employee employee, MerchPack merchPack)
        {
            if (!managers.Any(m => m.CanHandleNewTask))
            {
                throw new Exception("No vacant managers");
            }

            var responsibleManager = managers.OrderBy(m => m.AssignedTasks).First();

            var merchRequest = new MerchandizeRequest(responsibleManager.Id, responsibleManager.PhoneNumber, merchPack, DateTime.Now);
            
            merchRequest.AddEmployeeInfo(employee.Id, employee.PhoneNumber, employee.Size, DateTime.Now);

            responsibleManager.AssignTask();

            return merchRequest;
        }
    }
}
