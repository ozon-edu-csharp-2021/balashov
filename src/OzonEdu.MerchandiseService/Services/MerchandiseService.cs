using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseService.Models;

namespace OzonEdu.MerchandiseService.Services
{
    public class MerchandiseService : IMerchandiseService
    {
        public Task<MerchItemModel> RequestMerch(long personId, MerchItemRequestDto merchType, CancellationToken token)
        {
            throw new System.NotImplementedException();
        }

        public Task<IssuingMerchModel> GetMerchIssueInfo(long personId, CancellationToken token)
        {
            throw new System.NotImplementedException();
        }
    }
}