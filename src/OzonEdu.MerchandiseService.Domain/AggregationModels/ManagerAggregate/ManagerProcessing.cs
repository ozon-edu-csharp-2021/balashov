using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.ManagerAggregate
{
    public static class ManagerProcessing
    {
        public static async Task<Manager> GetManager(IManagerRepository managerRepository, long managerId, CancellationToken cancellationToken)
        {
            if (managerId > 0)
            {
                var manager = await managerRepository.FindByIdAsync(managerId, cancellationToken);
                return manager;
            }
            else
            {
                var managers = await managerRepository.GetAllAsync(cancellationToken);

                if (!managers.Any(m => m.CanHandleNewTask))
                {
                    throw new Exception("No vacant managers");
                }
                return managers.OrderBy(m => m.AssignedTasks).First();
            }
        }
    }
}
