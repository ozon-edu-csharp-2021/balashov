using System.Threading.Tasks;
using OzonEdu.MerchandiseService.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate;

namespace OzonEdu.MerchandiseService.Infrastructure.InterfacesToExternals
{
    public interface IEmailServer
    {
        public Task<bool> SendEmailAboutMerchAsync(long employeeId, string text);
    }
}
