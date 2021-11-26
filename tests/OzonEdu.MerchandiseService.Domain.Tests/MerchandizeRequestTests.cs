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
        private Date date = new Date(2021, 11, 05);

        private MerchandiseRequest MakeFakeRequest()
        {
            var hrManagerId = 1;
            var hrManagerContactPhone = new PhoneNumber("+123456789");
            var requestedMerchPack = new MerchPack(new MerchLine("testML", new Year(2021)), MerchPackTitle.WelcomePack);
            var result = new MerchandiseRequest(hrManagerId, requestedMerchPack, date);
            return result;
        }

        private MerchandiseRequest AddEmployee()
        {
            var request = MakeFakeRequest();
            var employeeId = 10;
            var employeeContactPhone = new PhoneNumber("+987654321");
            var size = Size.XL;
            var createdDate = new Date(2021, 11, 06);

            request.AddEmployeeInfo(employeeId, employeeContactPhone, size).SetCreated(createdDate);
            return request;
        }

        [Fact]
        public void Create_CorrectInput_StatusDraft()
        {
            var result = MakeFakeRequest();

            result.Status.Status.Should().Be(MerchRequestStatusType.Draft);
            result.Status.Date.Should().BeEquivalentTo(date);

        }

        [Fact]
        public void Create_AddEmployeeInfo_StatusCreated()
        {
            var request = MakeFakeRequest();
            var employeeId = 10;
            var employeeContactPhone = new PhoneNumber("+987654321");
            var size = Size.XL;
            var createdDate = new Date(2021, 11, 06);

            var result = request.AddEmployeeInfo(employeeId, employeeContactPhone, size).SetCreated(createdDate);

            result.Should().BeTrue();
            request.Status.Status.Should().Be(MerchRequestStatusType.Created);
            request.Status.Date.Should().BeEquivalentTo(createdDate);

        }

        [Fact]
        public void Create_SetAssignedForCreated_TrueStatusAssigned()
        {
            var request = AddEmployee();
            var assignedDate = new Date(2021, 12, 07);

            var result = request.SetAssigned(assignedDate);

            result.Should().BeTrue();
            request.Status.Status.Should().Be(MerchRequestStatusType.Assigned);
            request.Status.Date.Should().BeEquivalentTo(assignedDate);

        }

        [Fact]
        public void Create_SetAssignedForNotCreated_False()
        {
            var request = MakeFakeRequest();
            var assignedDate = new DateTime(2021, 12, 07);

            var result = request.SetAssigned(new Date(assignedDate));

            result.Should().BeFalse();
        }

        [Fact]
        public void Create_SetInProgressForAssigned_TrueStatusInProgress()
        {
            var request = AddEmployee();
            var assignedDate = new Date(2021, 11, 06);
            var inProgressDate = new Date(2021, 12, 08);
            request.SetAssigned(assignedDate);

            var result = request.SetReserved(inProgressDate);

            result.Should().BeTrue();
            request.Status.Status.Should().Be(MerchRequestStatusType.Reserved);
            request.Status.Date.Should().BeEquivalentTo(inProgressDate);
        }

        [Fact]
        public void Create_SetInProgressForNotAssigned_False()
        {
            var request = AddEmployee();
            var inProgressDate = new Date(2021, 12, 07);

            var result = request.SetReserved(inProgressDate);

            result.Should().BeFalse();
        }

        [Fact]
        public void Create_SetDoneForInProgress_TrueStatusDone()
        {
            var request = AddEmployee();
            var assignedDate = new Date(2021, 11, 06);
            var inProgressDate = new Date(2021, 12, 08);
            var doneDate = new Date(2021, 12, 10);
            request.SetAssigned(assignedDate);
            request.SetReserved(inProgressDate);

            var result = request.SetDone(doneDate);

            result.Should().BeTrue();
            request.Status.Status.Should().Be(MerchRequestStatusType.Done);
            request.Status.Date.Should().BeEquivalentTo(doneDate);
        }

        [Fact]
        public void Create_SetDoneForInProgress_False()
        {
            var request = AddEmployee();
            var doneDate = new Date(2021, 12, 07);

            var result = request.SetDone(doneDate);

            result.Should().BeFalse();
        }
    }
}
