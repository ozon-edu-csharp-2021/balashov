using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.ManagerAggregate
{
    public static class ManagerProcessing
    {
        public static async Task<Manager> GetTheManagerAsync(IManagerRepository managerRepository, long managerId, CancellationToken cancellationToken)
        {
            if (managerId <= 0) return null;

            var manager = await managerRepository.FindByIdAsync(managerId, cancellationToken);
            return manager;

        }

        public static async Task<Manager> GetTheManagerOrFreeManagerAsync(
            IManagerRepository managerRepository, long managerId, CancellationToken cancellationToken)
        {
            if (managerId > 0)
            {
                return await GetTheManagerAsync(managerRepository, managerId, cancellationToken);
            }

            var managers = await managerRepository.GetAllAsync(cancellationToken);

            var freeManagers = managers.FindAll(m => m.CanHandleNewTask);

            if (freeManagers.Count == 0)
                return null;

            if (freeManagers.Count == 1)
                return freeManagers.First();

            //Случайная выборка для более равномерного распределения заданий по менеджерам
            return freeManagers[new Random().Next(0, freeManagers.Count)];

        }
    }
}
