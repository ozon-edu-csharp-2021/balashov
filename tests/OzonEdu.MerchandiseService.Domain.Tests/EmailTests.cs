using System;
using FluentAssertions;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects;
using Xunit;

namespace OzonEdu.MerchandiseService.Domain.Tests
{
    /// <summary>http://haacked.com/archive/2007/08/21/i-knew-how-to-validate-an-email-address-until-i.aspx/ </summary>
    public class EmailTests
    {
        [Theory]
        [InlineData(@"NotAnEmail", false)]
        [InlineData(@"@NotAnEmail", false)]
        [InlineData(@"""test\\blah""@example.com", true)]
        [InlineData(@"""test\blah""@example.com", false)]
        [InlineData("\"test\\\rblah\"@example.com", true)]
        [InlineData("\"test\rblah\"@example.com", false)]
        [InlineData(@"""test\""blah""@example.com", true)]
        [InlineData(@"""test""blah""@example.com", false)]
        [InlineData(@"customer/department@example.com", true)]
        [InlineData(@"$A12345@example.com", true)]
        [InlineData(@"!def!xyz%abc@example.com", true)]
        [InlineData(@"_Yosemite.Sam@example.com", true)]
        [InlineData(@"~@example.com", true)]
        [InlineData(@".wooly@example.com", false)]
        [InlineData(@"wo..oly@example.com", false)]
        [InlineData(@"pootietang.@example.com", false)]
        [InlineData(@".@example.com", false)]
        [InlineData(@"""Austin@Powers""@example.com", true)]
        [InlineData(@"Ima.Fool@example.com", true)]
        [InlineData(@"""Ima.Fool""@example.com", true)]
        [InlineData(@"""Ima Fool""@example.com", true)]
        [InlineData(@"Ima Fool@example.com", false)]
        public void EmailValidationViaRegex(string email, bool expected)
        {
            var result = Email.EmailValidationViaRegex(email);
            result.Should().Be(expected);
        }

        [Fact]
        public void EmailCreate_EmailString_NewEmail()
        {
            var emailString = "test@example.com";
            var result = new Email(emailString);

            result.EmailString.Should().Be(emailString);
        }

        [Fact]
        public void EmailCreate_BadString_Exception()
        {
            var emailString = "notaemail.com";
            Assert.Throws<Exception>(() => { new Email(emailString); });
        }
    }
}
