using OzonEdu.MerchandiseService.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate;

namespace OzonEdu.MerchandiseService.Infrastructure.Interfaces
{
    public interface IEmailServer
    {
        public bool SendEmailAboutMerch(Employee employee, MerchandiseRequest merchRequest);
    }
}
