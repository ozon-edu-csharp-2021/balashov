using System;
using FluentAssertions;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects;
using Xunit;

namespace OzonEdu.MerchandiseService.Domain.Tests
{
    public class HeightTests
    {
        [Theory]
        [InlineData(70)]
        [InlineData(190)]
        [InlineData(250)]
        public void FromMetrics_CorrectHeight_Ok(int height)
        {
            var result = HeightMetric.FromMetrics(height);

            result.Centimeters.Should().Be(height);
        }

        [Theory]
        [InlineData(69)]
        [InlineData(0)]
        [InlineData(-5)]
        [InlineData(int.MinValue)]
        public void FromMetrics_TooSmallHeight_Exception(int height)
        {
            Assert.Throws<Exception>(() => { HeightMetric.FromMetrics(height); });
        }

        [Theory]
        [InlineData(251)]
        [InlineData(3000)]
        [InlineData(int.MaxValue)]
        public void FromMetrics_TooBigHeight_Exception(int height)
        {
            Assert.Throws<Exception>(() => { HeightMetric.FromMetrics(height); });
        }

        [Theory]
        [InlineData(7, 2)]
        [InlineData(3, 0)]
        [InlineData(0, 80)]
        public void FromImperial_CorrectHeight_Ok(int heightFeet, int heightInches)
        {
            var control = (int)Math.Round((heightFeet * 12 + heightInches) * 2.54);

            var result = HeightMetric.FromImperial(heightFeet, heightInches);

            result.Centimeters.Should().Be(control);
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 0)]
        public void FromImperial_TooSmallHeight_Exception(int heightFeet, int heightInches)
        {
            Assert.Throws<Exception>(() => { HeightMetric.FromImperial(heightFeet, heightInches); });
        }

        [Theory]
        [InlineData(0, -1)]
        [InlineData(-1, 0)]
        public void FromImperial_IncorrectHeight_Exception(int heightFeet, int heightInches)
        {
            Assert.Throws<Exception>(() => { HeightMetric.FromImperial(heightFeet, heightInches); });
        }

        [Theory]
        [InlineData(10,0)]
        [InlineData(0, 135)]
        public void FromImperial_TooBigHeight_Exception(int heightFeet, int heightInches)
        {
            Assert.Throws<Exception>(() => { HeightMetric.FromImperial(heightFeet, heightInches); });
        }
    }
}
