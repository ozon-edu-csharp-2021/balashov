using System;
using FluentAssertions;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects;
using Xunit;

namespace OzonEdu.MerchandiseService.Domain.Tests
{
    public class MonthTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(12)]
        public void Create_CorrectMonth_Ok(int month)
        {
            var result = new Month(month);

            result.TheMonth.Should().Be(month);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        [InlineData(int.MinValue)]
        public void Create_TooSmallMonth_Exception(int month)
        {
            Assert.Throws<Exception>(() => { new Month(month); });
        }

        [Theory]
        [InlineData(13)]
        [InlineData(3000)]
        [InlineData(int.MaxValue)]
        public void Create_TooBigMonth_Exception(int month)
        {
            Assert.Throws<Exception>(() => { new Month(month); });
        }
    }
}
