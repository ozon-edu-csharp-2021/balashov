using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseService.Domain.AggregationModels.Enumerations;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchandiseService.Domain.Contracts;

namespace OzonEdu.MerchandiseService.Infrastructure.Repositories.Implementation
{
    class MerchRepository : IMerchRepository
    {
        private List<MerchandiseRequest> fakeMerchandiseRequests;
        private int id = 1;
        public MerchRepository(IUnitOfWork unitOfWork)
        {
            fakeMerchandiseRequests = new List<MerchandiseRequest>();

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
    }
}
