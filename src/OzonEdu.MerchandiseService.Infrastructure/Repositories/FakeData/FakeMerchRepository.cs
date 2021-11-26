using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseService.Domain.AggregationModels.Enumerations;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchandiseService.Domain.Contracts;

namespace OzonEdu.MerchandiseService.Infrastructure.FakeData
{
    class FakeMerchRepository : IMerchRepository
    {
        private List<MerchandiseRequest> fakeMerchandiseRequests;
        private int id = 1;
        public FakeMerchRepository(IUnitOfWork unitOfWork)
        {
            fakeMerchandiseRequests = GetFakeMerchandiseRequests();

            UnitOfWork = unitOfWork;

            foreach (var fakeEmp in fakeMerchandiseRequests)
                fakeEmp.SetId(id++);
        }

        public IUnitOfWork UnitOfWork { get; }

        public async Task<MerchandiseRequest> CreateAsync(MerchandiseRequest itemToCreate, CancellationToken cancellationToken = default)
        {
            fakeMerchandiseRequests.Add(itemToCreate);
            itemToCreate.SetId(id++);

            return itemToCreate;
        }

        public async Task<MerchandiseRequest> UpdateAsync(MerchandiseRequest itemToUpdate, CancellationToken cancellationToken = default)
        {
            var index = fakeMerchandiseRequests.IndexOf(itemToUpdate);
            if (index == -1)
                throw new Exception("There is no such object in the repository");

            fakeMerchandiseRequests[index] = itemToUpdate;
            return itemToUpdate;
        }

        public async Task<MerchandiseRequest> FindByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            return fakeMerchandiseRequests.FirstOrDefault(m => m.Id == id);
        }

        public async Task<List<MerchandiseRequest>> FindByEmployeeIdAsync(long employeeId, CancellationToken cancellationToken = default)
        {
            return fakeMerchandiseRequests.FindAll(m => m.EmployeeId == employeeId);
        }
        
        private List<MerchandiseRequest> GetFakeMerchandiseRequests()
        {
            var mrList = new List<MerchandiseRequest>();
            //var mr111 = new MerchandiseRequest(1, new PhoneNumber("+012345678"), new MerchPack(10), new Date(2021, 11, 11));
            //mr111.AddEmployeeInfo(1, new PhoneNumber("+012345678"), Size.L, new Date(2020, 10, 10));
            //mr111.SetAssigned(new Date(2021, 11, 11));
            //mr111.SetReserved(new Date(2021, 11, 11));
            //mr111.SetDone(new Date(2021, 11, 11));
            //mrList.Add(mr111);

            //var mr112 = new MerchandiseRequest(1, new PhoneNumber("+012345678"), new MerchPack(10), new Date(2021, 11, 11));
            //mr112.AddEmployeeInfo(1, new PhoneNumber("+012345678"), Size.L, new Date(2021, 11, 11));
            //mr112.SetAssigned(new Date(2021, 11, 11));
            //mr112.SetReserved(new Date(2021, 11, 11));
            //mr112.SetDone(new Date(2021, 11, 11));
            //mrList.Add(mr112);

            //var mr12 = new MerchandiseRequest(1, new PhoneNumber("+012345678"), new MerchPack(20), new Date(2021, 11, 11));
            //mr12.AddEmployeeInfo(1, new PhoneNumber("+012345678"), Size.L, new Date(2021, 11, 11));
            //mr12.SetAssigned(new Date(2021, 11, 11));
            //mr12.SetReserved(new Date(2021, 11, 11));
            //mrList.Add(mr12);

            //var mr13 = new MerchandiseRequest(1, new PhoneNumber("+012345678"), new MerchPack(30), new Date(2021, 11, 11));
            //mr13.AddEmployeeInfo(1, new PhoneNumber("+012345678"), Size.L, new Date(2021, 11, 11));
            //mr13.SetAssigned(new Date(2021, 11, 11));
            //mrList.Add(mr13);

            //var mr221 = new MerchandiseRequest(2, new PhoneNumber("+012345678"), new MerchPack(20), new Date(2021, 09, 09));
            //mr221.AddEmployeeInfo(2, new PhoneNumber("+012345678"), Size.L, new Date(2021, 09, 10));
            //mr221.SetAssigned(new Date(2021, 09, 11));
            //mr221.SetReserved(new Date(2021, 09, 12));
            //mr221.SetDone(new Date(2021, 09, 13));
            //mrList.Add(mr221);

            //var mr231 = new MerchandiseRequest(2, new PhoneNumber("+012345678"), new MerchPack(30), new Date(2021, 09, 09));
            //mr231.AddEmployeeInfo(2, new PhoneNumber("+012345678"), Size.L, new Date(2021, 09, 10));
            //mr231.SetAssigned(new Date(2021, 09, 11));
            //mr231.SetReserved(new Date(2021, 09, 12));
            //mr231.SetDone(new Date(2021, 09, 13));
            //mrList.Add(mr231);

            return mrList;
        }
    }
}
