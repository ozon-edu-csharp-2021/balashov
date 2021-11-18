using System.Collections.Generic;
using OzonEdu.MerchandiseService.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.Enumerations;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects;

namespace OzonEdu.MerchandiseService.Domain.DomainServices
{
    public sealed class MerchDomainService
    {
        public const int DaysBetweenIssuance = 365;

        public static bool IsEmployeeReceivedMerchLastTime (
            List<MerchandiseRequest> oneTypeIssuedMerch, 
            Employee employee, 
            Date todayDate, 
            out string whyString)
        {
            whyString = string.Empty;

            if (oneTypeIssuedMerch.Count == 0) return true;

            var lastIssuedMerch = oneTypeIssuedMerch[0];
            var minDays = lastIssuedMerch.Status.Date.CountDeltaDays(todayDate);
            for (int i = 1; i < oneTypeIssuedMerch.Count; i++)
            {
                var days = oneTypeIssuedMerch[i].Status.Date.CountDeltaDays(todayDate);
                if (days < minDays)
                {
                    minDays = days;
                    lastIssuedMerch = oneTypeIssuedMerch[i];
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
                if (deltaDays < DaysBetweenIssuance)
                {
                    whyString = $"Сотрудник {employee.Name} уже получал такой мерч менее года назад (последний раз {deltaDays} дней назад)";
                    return false;
                }
            }

            return true;
        }
    }
}
