using System;
using FluentAssertions;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects;
using Xunit;

namespace OzonEdu.MerchandiseService.Domain.Tests
{
    public class PhoneNumberTests
    {
        [Theory]
        [InlineData("+123456789")]
        [InlineData("+723456789")]
        [InlineData("+000000000")]
        public void Create_CorrectPhoneNumber_Ok(string phone)
        {
            var result = new PhoneNumber(phone);

            result.Phone.Should().Be(phone);
        }

        [Theory]
        [InlineData("20")]
        [InlineData("123456789")]
        [InlineData("-723456789")]
        [InlineData("телефон")]
        public void Create_IncorrectPhoneNumber_Exception(string phone)
        {
            Assert.Throws<Exception>(() => { new PhoneNumber(phone); });
        }
    }
}
