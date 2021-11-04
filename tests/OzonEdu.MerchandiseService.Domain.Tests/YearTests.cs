using System;
using FluentAssertions;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects;
using Xunit;

namespace OzonEdu.MerchandiseService.Domain.Tests
{
    public class YearTests
    {
        [Theory]
        [InlineData(2020)]
        [InlineData(2021)]
        [InlineData(2035)]
        [InlineData(2100)]
        public void Create_CorrectYear_Ok(int year)
        {
            var result = new Year(year);

            result.TheYear.Should().Be(year);
        }

        [Theory]
        [InlineData(2019)]
        [InlineData(2000)]
        [InlineData(0)]
        [InlineData(-10)]
        [InlineData(int.MinValue)]
        public void Create_TooSmallYear_Exception(int year)
        {
            Assert.Throws<Exception>(() => { new Year(year); });
        }

        [Theory]
        [InlineData(2101)]
        [InlineData(3000)]
        [InlineData(int.MaxValue)]
        public void Create_TooBigYear_Exception(int year)
        {
            Assert.Throws<Exception>(() => { new Year(year); });
        }
    }
}
