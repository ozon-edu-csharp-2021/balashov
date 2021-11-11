using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ManagerAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchandiseService.Domain.Contracts;

namespace OzonEdu.MerchandiseService.Infrastructure.FakeData
{
    class FakeManagerRepository : IManagerRepository
    {
        private List<Manager> fakeManagers;
        private int id = 1;
        public FakeManagerRepository(IUnitOfWork unitOfWork)
        {
            fakeManagers = GetFakeManagers();

            UnitOfWork = unitOfWork;

            foreach (var fakeManager in fakeManagers)
                fakeManager.SetId(id++);
        }

        public IUnitOfWork UnitOfWork { get; }

        public async Task<Manager> CreateAsync(Manager itemToCreate, CancellationToken cancellationToken = default)
        {
            fakeManagers.Add(itemToCreate);
            itemToCreate.SetId(id++);

            return itemToCreate;
        }

        public async Task<Manager> UpdateAsync(Manager itemToUpdate, CancellationToken cancellationToken = default)
        {
            var managerIndex = fakeManagers.IndexOf(itemToUpdate);
            if (managerIndex == -1)
                throw new Exception("There is no such object in the repository");

            fakeManagers[managerIndex] = itemToUpdate;
            return itemToUpdate;
        }

        public async Task<Manager> FindByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            return fakeManagers.FirstOrDefault(m => m.Id == id);
        }

        public async Task<List<Manager>> FindByNameIdAsync(PersonName personName, CancellationToken cancellationToken = default)
        {
            return fakeManagers.FindAll(m => m.Name.Equals(personName));
        }

        public async Task<List<Manager>> GetAll(CancellationToken cancellationToken = default)
        {
            return fakeManagers;
        }

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
    }
}
