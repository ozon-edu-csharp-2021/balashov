using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseService.Models;

namespace OzonEdu.MerchandiseService.Services
{
    public interface IMerchandiseService
    {
        Task<MerchItemModel> RequestMerch(long personId, MerchItemRequestDto merchType, CancellationToken token);
        Task<IssuingMerchModel> GetMerchIssueInfo(long personId, CancellationToken token);
    }
}