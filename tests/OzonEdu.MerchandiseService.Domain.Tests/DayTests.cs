using System;
using FluentAssertions;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects;
using Xunit;

namespace OzonEdu.MerchandiseService.Domain.Tests
{
    public class DayTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(13)]
        [InlineData(31)]
        public void Create_CorrectDay_Ok(int day)
        {
            var result = new Day(day);

            result.TheDay.Should().Be(day);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        [InlineData(int.MinValue)]
        public void Create_TooSmallDay_Exception(int day)
        {
            Assert.Throws<Exception>(() => { new Day(day); });
        }

        [Theory]
        [InlineData(32)]
        [InlineData(3000)]
        [InlineData(int.MaxValue)]
        public void Create_TooBigDay_Exception(int day)
        {
            Assert.Throws<Exception>(() => { new Day(day); });
        }
    }
}
