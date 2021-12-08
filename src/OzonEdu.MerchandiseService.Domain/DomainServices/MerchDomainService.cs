using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseService.Domain.AggregationModels.Enumerations;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ManagerAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects;

namespace OzonEdu.MerchandiseService.Domain.DomainServices
{
    public sealed class MerchDomainService
    {
        public const int DaysBetweenIssuance = 365;

        public static bool CanEmployeeReceiveNewMerch(
            List<MerchandiseRequest> allMerchRequestForEmployee,
            MerchPack requestedMerchPack,
            Date todayDate,
            out string whyNotString)
        {
            whyNotString = string.Empty;

            var oneTypeIssuedMerch = allMerchRequestForEmployee.FindAll(mr => mr.RequestedMerchPack.Equals(requestedMerchPack));

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
                whyNotString = $"Сотрудник уже ожидает получение такого мерча";
                return false;
            }

            if (lastIssuedMerch.Status.Status.Equals(MerchRequestStatusType.Done))
            {
                var deltaDays = lastIssuedMerch.Status.Date.CountDeltaDays(todayDate);
                if (deltaDays < DaysBetweenIssuance)
                {
                    whyNotString =
                        $"Сотрудник уже получал такой мерч менее года назад (последний раз {deltaDays} дней назад)";
                    return false;
                }
            }

            return true;
        }

        public static async Task MerchRequestAssignTaskForHr(
            MerchandiseRequest merchRequest, 
            IManagerRepository managerRepository, 
            CancellationToken cancellationToken)
        {
            var date = new Date(DateTime.Now);

            var manager = await ManagerProcessing.GetTheManagerOrFreeManagerAsync(managerRepository,
                merchRequest.HRManagerId, cancellationToken);

            if (manager == null)
            {
                var text = merchRequest.HRManagerId > 0
                    ? $"Не удалось найти менеджера с id:{merchRequest.HRManagerId}"
                    : "Не удалось найти свободного менеджера";
                throw new Exception(text);
            }

            manager.AssignTask();
            await managerRepository.UpdateAssignedTasksAsync(manager);

            if (merchRequest.HRManagerId != manager.Id)
                merchRequest.SetManagerId(manager.Id);

            if (!merchRequest.SetAssigned(date))
                throw new Exception($"Заявка id:{merchRequest.Id} не назначена HR-менеджеру! Метод вернул отказ");
        }
    }

}
