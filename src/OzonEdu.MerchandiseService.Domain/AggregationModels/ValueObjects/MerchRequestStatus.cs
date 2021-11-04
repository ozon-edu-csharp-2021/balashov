using System;
using System.Collections.Generic;
using OzonEdu.MerchandiseService.Domain.AggregationModels.Enumerations;
using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects
{
    public class MerchRequestStatus : ValueObject
    {
        public MerchRequestStatusType Status { get; }

        public Year Year { get; }

        public Month Month { get; }

        public Day Day { get; }

        public MerchRequestStatus(MerchRequestStatusType status, DateTime dateTime)
        {
            Status = status;
            Year = new Year(dateTime.Year);
            Month = new Month(dateTime.Month);
            Day = new Day(dateTime.Day);
        }

        public MerchRequestStatus(MerchRequestStatusType status, Year year, Month month, Day day)
        {
            Status = status;
            Year = year;
            Month = month;
            Day = day;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Status;
            yield return Year;
            yield return Month;
            yield return Day;
        }
    }
}
