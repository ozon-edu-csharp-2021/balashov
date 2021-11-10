using System;
using System.Collections.Generic;
using OzonEdu.MerchandiseService.Domain.AggregationModels.Enumerations;
using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects
{
    public class MerchRequestStatus : ValueObject
    {
        public MerchRequestStatusType Status { get; }

        public Date Date { get; }

        public MerchRequestStatus(MerchRequestStatusType status, Date date)
        {
            Status = status;
            Date = date;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Status;
            yield return Date;
        }

        public override string ToString()
        {
            return $"Статус заявки: {Status.Name}, дата последнего изменения: {Date}";
        }
    }
}
