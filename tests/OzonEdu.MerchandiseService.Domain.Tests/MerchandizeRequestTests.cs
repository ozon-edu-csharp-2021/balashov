using System;
using FluentAssertions;
using OzonEdu.MerchandiseService.Domain.AggregationModels.Enumerations;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects;
using Xunit;

namespace OzonEdu.MerchandiseService.Domain.Tests
{
    public class MerchandizeRequestTests
    {
        private DateTime dateTime = new DateTime(2021, 11, 05);

        private MerchandizeRequest MakeFakeRequest()
        {
            var hrManagerId = 1;
            var hrManagerContactPhone = new PhoneNumber("+123456789");
            var requestedMerchPack = new MerchPack(new MerchLine("testML", 2021), MerchPackTitle.WelcomePack);
            var result = new MerchandizeRequest(hrManagerId, hrManagerContactPhone, requestedMerchPack, dateTime);
            return result;
        }

        private MerchandizeRequest AddEmployee()
        {
            var request = MakeFakeRequest();
            var employeeId = 10;
            var employeeContactPhone = new PhoneNumber("+987654321");
            var size = Size.XL;
            var createdDateTime = new DateTime(2021, 11, 06);

            request.AddEmployeeInfo(employeeId, employeeContactPhone, size, createdDateTime);
            return request;
        }

        [Fact]
        public void Create_CorrectInput_StatusDraft()
        {
            var result = MakeFakeRequest();

            result.Status.Status.Should().Be(MerchRequestStatusType.Draft);
            result.Status.Year.TheYear.Should().Be(dateTime.Year);
            result.Status.Month.TheMonth.Should().Be(dateTime.Month);
            result.Status.Day.TheDay.Should().Be(dateTime.Day);
        }

        [Fact]
        public void Create_AddEmployeeInfo_StatusCreated()
        {
            var request = MakeFakeRequest();
            var employeeId = 10;
            var employeeContactPhone = new PhoneNumber("+987654321");
            var size = Size.XL;
            var createdDateTime = new DateTime(2021, 11, 06);

            var result = request.AddEmployeeInfo(employeeId, employeeContactPhone, size, createdDateTime);

            result.Should().BeTrue();
            request.Status.Status.Should().Be(MerchRequestStatusType.Created);
            request.Status.Year.TheYear.Should().Be(createdDateTime.Year);
            request.Status.Month.TheMonth.Should().Be(createdDateTime.Month);
            request.Status.Day.TheDay.Should().Be(createdDateTime.Day);
        }

        [Fact]
        public void Create_SetAssignedForCreated_TrueStatusAssigned()
        {
            var request = AddEmployee();
            var assignedDateTime = new DateTime(2021, 12, 07);

            var result = request.SetAssigned(assignedDateTime);

            result.Should().BeTrue();
            request.Status.Status.Should().Be(MerchRequestStatusType.Assigned);
            request.Status.Year.TheYear.Should().Be(assignedDateTime.Year);
            request.Status.Month.TheMonth.Should().Be(assignedDateTime.Month);
            request.Status.Day.TheDay.Should().Be(assignedDateTime.Day);
        }

        [Fact]
        public void Create_SetAssignedForNotCreated_False()
        {
            var request = MakeFakeRequest();
            var assignedDateTime = new DateTime(2021, 12, 07);

            var result = request.SetAssigned(assignedDateTime);

            result.Should().BeFalse();
        }

        [Fact]
        public void Create_SetInProgressForAssigned_TrueStatusInProgress()
        {
            var request = AddEmployee();
            var assignedDateTime = new DateTime(2021, 11, 06);
            var inProgressDateTime = new DateTime(2021, 12, 08);
            request.SetAssigned(assignedDateTime);

            var result = request.SetInProgress(inProgressDateTime);

            result.Should().BeTrue();
            request.Status.Status.Should().Be(MerchRequestStatusType.InProgress);
            request.Status.Year.TheYear.Should().Be(inProgressDateTime.Year);
            request.Status.Month.TheMonth.Should().Be(inProgressDateTime.Month);
            request.Status.Day.TheDay.Should().Be(inProgressDateTime.Day);
        }

        [Fact]
        public void Create_SetInProgressForNotAssigned_False()
        {
            var request = AddEmployee();
            var inProgressDateTime = new DateTime(2021, 12, 07);

            var result = request.SetInProgress(inProgressDateTime);

            result.Should().BeFalse();
        }

        [Fact]
        public void Create_SetDoneForInProgress_TrueStatusDone()
        {
            var request = AddEmployee();
            var assignedDateTime = new DateTime(2021, 11, 06);
            var inProgressDateTime = new DateTime(2021, 12, 08);
            var doneDateTime = new DateTime(2021, 12, 10);
            request.SetAssigned(assignedDateTime);
            request.SetInProgress(inProgressDateTime);

            var result = request.SetDone(doneDateTime);

            result.Should().BeTrue();
            request.Status.Status.Should().Be(MerchRequestStatusType.Done);
            request.Status.Year.TheYear.Should().Be(doneDateTime.Year);
            request.Status.Month.TheMonth.Should().Be(doneDateTime.Month);
            request.Status.Day.TheDay.Should().Be(doneDateTime.Day);
        }

        [Fact]
        public void Create_SetDoneForInProgress_False()
        {
            var request = AddEmployee();
            var doneDateTime = new DateTime(2021, 12, 07);

            var result = request.SetDone(doneDateTime);

            result.Should().BeFalse();
        }
    }
}
