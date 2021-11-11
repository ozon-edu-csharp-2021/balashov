using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseService.Models;

namespace OzonEdu.MerchandiseService.HttpClient
{
    public class MerchandiseHttpClient
    {
        private readonly System.Net.Http.HttpClient _httpClient;
        private const string BaseRoute = "api/v1/merchandise/";

        public MerchandiseHttpClient(System.Net.Http.HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<MerchandiseRequestResponseDto> GetIssuedMerchInfo(long employeeId, MerchandiseRequestRequestDto merchType, CancellationToken token)
        {
            var requestUri = $"{BaseRoute}employee/{employeeId}";

            var json = JsonSerializer.Serialize(merchType);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using var response = await _httpClient.PostAsync(requestUri, content, token);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var body = await response.Content.ReadAsStringAsync(token);
                return JsonSerializer.Deserialize<MerchandiseRequestResponseDto>(body);
            }

            return null;
        }

        public async Task<bool> RequestMerch(long employeeId, CancellationToken token)
        {
            var requestUri = $"{BaseRoute}employee/{employeeId}";

            using var response = await _httpClient.GetAsync(requestUri, token);
            if (response.StatusCode == HttpStatusCode.OK)
                return true;

            return false;
        }
    }
}
