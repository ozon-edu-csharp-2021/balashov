using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using OzonEdu.MerchandiseService.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.Enumerations;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ManagerAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchandiseService.Domain.DomainServices;
using Xunit;

namespace OzonEdu.MerchandiseService.Domain.Tests
{
    public class MerchDomainServiceTests
    {

        private List<Manager> GetFakeManagers()
        {
            var managers = new List<Manager>();
            managers.Add(new Manager(
                PersonName.Create("testFirstManagerName1", "testLastManagerName1"),
                new Email("email1@test.t"), 
                new PhoneNumber("+012345678"), 
                5));
            managers.Add(new Manager(
                PersonName.Create("testFirstManagerName2", "testLastManagerName2"),
                new Email("email2@test.t"),
                new PhoneNumber("+012345678"),
                4));
            managers.Add(new Manager(
                PersonName.Create("testFirstManagerName3", "testLastManagerName3"),
                new Email("email3@test.t"),
                new PhoneNumber("+012345678"),
                3));
            managers.Add(new Manager(
                PersonName.Create("testFirstManagerName4", "testLastManagerName4"),
                new Email("email4@test.t"),
                new PhoneNumber("+012345678"),
                2));
            managers.Add(new Manager(
                PersonName.Create("testFirstManagerName5", "testLastManagerName5"),
                new Email("email5@test.t"),
                new PhoneNumber("+012345678"),
                1));

            return managers;
        }

        private Employee GetFakeEmployee()
        {
            return new Employee(
                PersonName.Create("testFirstEmployeeName", "testLastEmployeeName"),
                new Email("email4@test.t"),
                new PhoneNumber("+012345678"),
                Size.L,
                HeightMetric.FromMetrics(180));
        }

        private List<MerchandiseRequest> GetFakeMerchandiseRequests()
        {
            var mrList = new List<MerchandiseRequest>();
            var mr11 = new MerchandiseRequest(1, new PhoneNumber("+012345678"), new MerchPack(10), new Date(2021, 11, 11));
            mr11.AddEmployeeInfo(1, new PhoneNumber("+012345678"), Size.L, new Date(2020, 10, 10));
            mr11.SetAssigned(new Date(2021, 11, 11));
            mr11.SetInProgress(new Date(2021, 11, 11));
            mr11.SetDone(new Date(2021, 11, 11));
            mrList.Add(mr11);

            var mr12 = new MerchandiseRequest(1, new PhoneNumber("+012345678"), new MerchPack(10),new Date(2021, 11, 11 ));
            mr12.AddEmployeeInfo(1, new PhoneNumber("+012345678"), Size.L, new Date(2021, 11, 11));
            mr12.SetAssigned(new Date(2021, 11, 11));
            mr12.SetInProgress(new Date(2021, 11, 11));
            mr12.SetDone(new Date(2021, 11, 11));
            mrList.Add(mr12);

            var mr2 = new MerchandiseRequest(1, new PhoneNumber("+012345678"), new MerchPack(20), new Date(2021, 11, 11));
            mr2.AddEmployeeInfo(1, new PhoneNumber("+012345678"), Size.L, new Date(2021, 11, 11));
            mr2.SetAssigned(new Date(2021, 11, 11));
            mr2.SetInProgress(new Date(2021, 11, 11));
            mrList.Add(mr2);

            var mr3 = new MerchandiseRequest(1, new PhoneNumber("+012345678"), new MerchPack(30), new Date(2021, 11, 11));
            mr3.AddEmployeeInfo(1, new PhoneNumber("+012345678"), Size.L, new Date(2021, 11, 11));
            mr3.SetAssigned(new Date(2021, 11, 11));
            mrList.Add(mr3);

            return mrList;
        }

        [Fact]
        public void CreateMerchandiseRequest_CorrectInput_Created()
        {
            var managers = GetFakeManagers();
            var employee = GetFakeEmployee();

            var result = MerchDomainService.CreateMerchandiseRequest(managers, employee, new MerchPack(10));

            result.Status.Status.Should().Be(MerchRequestStatusType.Created);
        }

        [Fact]
        public void CreateMerchandiseRequest_MaxAssignedTasks_Exceptions()
        {
            var managers = GetFakeManagers().FindAll(m => m.AssignedTasks >= Manager.MaxTasksCount);
            var employee = GetFakeEmployee();

            Assert.Throws<Exception>(() => { MerchDomainService.CreateMerchandiseRequest(managers, employee, new MerchPack(10)); });
        }

        [Fact]
        public void CreateMerchandiseRequest_NoFreeManagers_Exceptions()
        {
            var managers = GetFakeManagers().FindAll(m => m.AssignedTasks > Manager.MaxTasksCount);
            var employee = GetFakeEmployee();

            Assert.Throws<Exception>(() => { MerchDomainService.CreateMerchandiseRequest(managers, employee, new MerchPack(10)); });
        }

        [Fact]
        public void CreateMerchandiseRequestOneManager_FreeManager_Created()
        {
            var manager = GetFakeManagers().Find(m => m.AssignedTasks < Manager.MaxTasksCount);
            var employee = GetFakeEmployee();

            var result = MerchDomainService.CreateMerchandiseRequest(manager, employee, new MerchPack(10));

            result.Status.Status.Should().Be(MerchRequestStatusType.Created);
        }

        [Fact]
        public void CreateMerchandiseRequestOneManager_NotFreeManager_Created()
        {
            var manager = GetFakeManagers().Find(m => m.AssignedTasks > Manager.MaxTasksCount);
            var employee = GetFakeEmployee();

            var result = MerchDomainService.CreateMerchandiseRequest(manager, employee, new MerchPack(10));

            result.Status.Status.Should().Be(MerchRequestStatusType.Created);
        }

        [Fact]
        public void IsEmployeeReceivedMerchLastTime_NewMerchType_True()
        {
            var employee = GetFakeEmployee();
            employee.SetId(1);
            var issuedMerch = GetFakeMerchandiseRequests().FindAll(m => m.RequestedMerchPack.PackTitle.Id == 40);

            var result = MerchDomainService.IsEmployeeReceivedMerchLastTime(
                issuedMerch,
                employee,
                new Date(2021, 11, 11),
                out string whyString);

            result.Should().BeTrue();
            whyString.Should().BeEmpty();
        }

        [Fact]
        public void IsEmployeeReceivedMerchLastTime_EnoughTimePassed_True()
        {
            var employee = GetFakeEmployee();
            employee.SetId(1);
            var issuedMerch = GetFakeMerchandiseRequests().FindAll(m => m.RequestedMerchPack.PackTitle.Id == 10);

            var result = MerchDomainService.IsEmployeeReceivedMerchLastTime(
                issuedMerch,
                employee,
                new Date(2022, 12, 12),
                out string whyString);

            result.Should().BeTrue();
            whyString.Should().BeEmpty();
        }

        [Fact]
        public void IsEmployeeReceivedMerchLastTime_MerchInProgress_False()
        {
            var employee = GetFakeEmployee();
            employee.SetId(1);
            var issuedMerch = GetFakeMerchandiseRequests()
                .FindAll(m => m.RequestedMerchPack.PackTitle.Id == 20 
                              && m.Status.Status.Equals( MerchRequestStatusType.InProgress));

            var result = MerchDomainService.IsEmployeeReceivedMerchLastTime(
                issuedMerch,
                employee,
                new Date(2021, 11, 12),
                out string whyString);

            result.Should().BeFalse();
            whyString.Should().Contain("ожидает получение");
        }

        [Fact]
        public void IsEmployeeReceivedMerchLastTime_NotEnoughTimePassed_False()
        {
            var employee = GetFakeEmployee();
            employee.SetId(1);
            var issuedMerch = GetFakeMerchandiseRequests().FindAll(m => m.RequestedMerchPack.PackTitle.Id == 10);

            var result = MerchDomainService.IsEmployeeReceivedMerchLastTime(
                issuedMerch,
                employee,
                new Date(2021, 11, 12),
                out string whyString);

            result.Should().BeFalse();
            whyString.Should().Contain("уже получал");
        }
    }
}
