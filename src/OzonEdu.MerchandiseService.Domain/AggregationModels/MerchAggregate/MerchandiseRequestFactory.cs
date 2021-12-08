using System;
using OzonEdu.MerchandiseService.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.Enumerations;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate
{
    public sealed class MerchandiseRequestFactory
    {
        public static MerchandiseRequest NewMerchandiseRequest(long managerId, Employee employee, Size size, MerchPack merchPack, Date date)
        {
            var merchRequest = new MerchandiseRequest(managerId, merchPack, date);

            merchRequest.AddEmployeeInfo(employee.Email, size);

            if (!merchRequest.SetCreated(date))
                throw new Exception("Заявка не сформирована! Метод вернул отказ");

            return merchRequest;
        }
    }
}
