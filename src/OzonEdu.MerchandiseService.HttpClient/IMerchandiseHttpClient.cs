using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseService.Models;

namespace OzonEdu.MerchandiseService.HttpClient
{
    public interface IMerchandiseHttpClient
    {
        Task<IssuedMerchInfoResponseDto> GetIssuedMerchInfo(
            long personId, 
            MerchItemRequestDto merchType, 
            CancellationToken token);

        Task<bool> RequestMerch(
            long personId, 
            CancellationToken token);
    }
}