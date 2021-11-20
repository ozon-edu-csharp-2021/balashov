using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseService.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.Enumerations;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchandiseService.Domain.Contracts;

namespace OzonEdu.MerchandiseService.Infrastructure.FakeData
{
    class FakeEmployeeRepository : IEmployeeRepository
    {
        private List<Employee> fakeEmployees;
        private int id = 1;
        public FakeEmployeeRepository(IUnitOfWork unitOfWork)
        {
            fakeEmployees = GetFakeEmployees();
            
            UnitOfWork = unitOfWork;

            foreach (var fakeEmp in fakeEmployees)
                fakeEmp.SetId(id++);
        }

        public IUnitOfWork UnitOfWork { get; }

        public async Task<Employee> CreateAsync(Employee itemToCreate, CancellationToken cancellationToken = default)
        {
            fakeEmployees.Add(itemToCreate);
            itemToCreate.SetId(id++);

            return itemToCreate;
        }

        public async Task<Employee> UpdateAsync(Employee itemToUpdate, CancellationToken cancellationToken = default)
        {
            var index = fakeEmployees.IndexOf(itemToUpdate);
            if (index == -1)
                throw new Exception("There is no such object in the repository");

            fakeEmployees[index] = itemToUpdate;
            return itemToUpdate;
        }

        public async Task<Employee> FindByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            return fakeEmployees.FirstOrDefault(m => m.Id == id);
        }

        public async Task<List<Employee>> FindByNameIdAsync(PersonName personName, CancellationToken cancellationToken = default)
        {
            return fakeEmployees.FindAll(m => m.Name.Equals(personName));
        }

        public async Task<List<Employee>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return fakeEmployees;
        }

        private List<Employee> GetFakeEmployees()
        {
            var employees = new List<Employee>();

            employees.Add(new Employee(
                PersonName.Create("testFirstEmployeeName", "testLastEmployeeName"),
                new Email("email@test.ru"),
                new PhoneNumber("+012345678")));

            employees.Add(new Employee(
                PersonName.Create("testFirstEmployeeName2", "testLastEmployeeName2"),
                new Email("email2@test.ru"),
                new PhoneNumber("+012345678")));

            employees.Add(new Employee(
                PersonName.Create("testFirstEmployeeName3", "testLastEmployeeName3"),
                new Email("email3@test.ru"),
                new PhoneNumber("+012345678")));

            employees.Add(new Employee(
                PersonName.Create("testFirstEmployeeName4", "testLastEmployeeName4"),
                new Email("email4@test.ru"),
                new PhoneNumber("+012345678")));

            employees.Add(new Employee(
                PersonName.Create("testFirstEmployeeName5", "testLastEmployeeName5"),
                new Email("email5@test.ru"),
                new PhoneNumber("+012345678")));

            return employees;
        }

    }
}
