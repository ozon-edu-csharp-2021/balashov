using System;
using System.Collections.Generic;
using System.Linq;
using OzonEdu.MerchandiseService.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.Enumerations;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ManagerAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects;

namespace OzonEdu.MerchandiseService.Domain.DomainServices
{
    public sealed class MerchDomainService
    {
        public static MerchandiseRequest CreateMerchandiseRequest(IEnumerable<Manager> managers, Employee employee, MerchPack merchPack)
        {
            if (!managers.Any(m => m.CanHandleNewTask))
            {
                throw new Exception("No vacant managers");
            }

            var responsibleManager = managers.OrderBy(m => m.AssignedTasks).First();

            return CreateMerchandiseRequest(responsibleManager, employee, merchPack);
        }

        public static MerchandiseRequest CreateMerchandiseRequest(Manager manager, Employee employee, MerchPack merchPack)
        {
            var date = new Date(DateTime.Now);

            var merchRequest = new MerchandiseRequest(manager.Id, manager.PhoneNumber, merchPack, date);

            merchRequest.AddEmployeeInfo(employee.Id, employee.PhoneNumber, employee.Size, date);

            manager.AssignTask();

            return merchRequest;
        }

        private const int daysBetweenIssuance = 365;

        public static bool IsEmployeeReceivedMerchLastTime (
            List<MerchandiseRequest> issuedMerchs, 
            Employee employee, 
            Date todayDate, 
            out string whyString)
        {
            whyString = string.Empty;

            if (issuedMerchs.Count == 0) return true;

            var lastIssuedMerch = issuedMerchs[0];
            var minDays = lastIssuedMerch.Status.Date.CountDeltaDays(todayDate);
            for (int i = 1; i < issuedMerchs.Count; i++)
            {
                var days = issuedMerchs[i].Status.Date.CountDeltaDays(todayDate);
                if (days < minDays)
                {
                    minDays = days;
                    lastIssuedMerch = issuedMerchs[i];
                }
            }

            if (!lastIssuedMerch.Status.Status.Equals(MerchRequestStatusType.Done))
            {
                whyString = $"Сотрудник {employee.Name} уже ожидает получение такого мерча";
                return false;
            }

            if (lastIssuedMerch.Status.Status.Equals(MerchRequestStatusType.Done))
            {
                var deltaDays = lastIssuedMerch.Status.Date.CountDeltaDays(todayDate);
                if (deltaDays < daysBetweenIssuance)
                {
                    whyString = $"Сотрудник {employee.Name} уже получал такой мерч менее года назад (последний раз {deltaDays} дней назад)";
                    return false;
                }
            }

            return true;
        }
    }
}
