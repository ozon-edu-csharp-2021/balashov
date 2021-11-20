using System;
using System.Collections.Generic;
using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects
{
    public class Date : ValueObject
    {
        private readonly Year _year;

        private readonly Month _month;

        private readonly Day _day;

        public int Year => _year.TheYear;

        public int Month => _month.TheMonth;

        public int Day => _day.TheDay;

        public Date(DateTime dateTime)
        {
            _year = new Year(dateTime.Year);
            _month = new Month(dateTime.Month);
            _day = new Day(dateTime.Day);
        }

        public Date(int year, int month, int day)
        {
            _year = new Year(year);
            _month = new Month(month);
            _day = new Day(day);
        }

        public Date(Year year, Month month, Day day)
        {
            _year = year;
            _month = month;
            _day = day;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Year;
            yield return Month;
            yield return Day;
        }

        public int CountDeltaDays(DateTime date)
        {
            return CountDeltaDaysBase(date.Year, date.Month, date.Day);
        }

        public int CountDeltaDays(Date date)
        {
            return CountDeltaDaysBase(date.Year, date.Month, date.Day);
        }

        private int CountDeltaDaysBase(int year, int month, int day)
        {
            var startDate = new DateTime(this.Year, this.Month, this.Day);
            var endDate = new DateTime(year, month, day);

            return (endDate.Date - startDate.Date).Days;
        }

        public DateTime ToDateTime()
        {
            return new DateTime(Year, Month, Day);
        }

        public override string ToString()
        {
            return $"{Year}-{Month}-{Day}";
        }
    }
}