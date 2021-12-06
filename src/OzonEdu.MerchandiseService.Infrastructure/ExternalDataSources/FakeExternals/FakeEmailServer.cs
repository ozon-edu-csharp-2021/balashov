using System;
using System.Threading.Tasks;
using OzonEdu.MerchandiseService.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ManagerAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate;
using OzonEdu.MerchandiseService.Infrastructure.ExternalDataSources;

namespace OzonEdu.MerchandiseService.Infrastructure.InterfacesToExternals.FakeExternals
{
    public class FakeEmailServer :IEmailServer
    {
        public Task SendEmailAboutMerchAsync(Employee employee, Manager manager, MerchandiseRequest merchRequest)
        {
            Console.WriteLine($"Понарошку отправляю письмо пользователю с id:{employee.Id}");
            return Task.CompletedTask;
        }
    }
}
