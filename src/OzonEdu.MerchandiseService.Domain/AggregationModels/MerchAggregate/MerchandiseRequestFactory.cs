using OzonEdu.MerchandiseService.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.Enumerations;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ManagerAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate
{
    public sealed class MerchandiseRequestFactory
    {
        public static MerchandiseRequest Create(Manager manager, Employee employee, Size size, MerchPack merchPack, Date date)
        {
            var merchRequest = new MerchandiseRequest(manager.Id, manager.PhoneNumber, merchPack, date);

            merchRequest.AddEmployeeInfo(employee.Id, employee.PhoneNumber, size, date);

            manager.AssignTask();

            return merchRequest;
        }
    }
}
