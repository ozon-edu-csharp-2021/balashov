using System.Globalization;
using System.Threading.Tasks;
using Grpc.Core;
using OzonEdu.MerchandiseService.Grpc;
using OzonEdu.MerchandiseService.Models;
using OzonEdu.MerchandiseService.Services;

namespace OzonEdu.MerchandiseService.GrpcServices
{
    public class MerchGrpcService : MerchGrpc.MerchGrpcBase
    {
        private readonly IMerchService _merchService;

        public MerchGrpcService(IMerchService merchService)
        {
            _merchService = merchService;
        }

        public override async Task<GetIssuedMerchInfoResponse> GetIssuedMerchInfo(GetIssuedMerchInfoRequest request, ServerCallContext context)
        {
            var token = context.CancellationToken;
            var personId = request.PersonId;

            var issuedMerch = await _merchService.GetIssuedMerchInfo(personId, token);

            if (issuedMerch == null)
                throw new RpcException(
                    new Status(StatusCode.NotFound, $"Merch info for person (id:{personId}) was not found"));

            var issuedMerchResponse = new GetIssuedMerchInfoResponse
            {
                Id = issuedMerch.Id,
                PersonId = issuedMerch.PersonId,
                IssueDate = issuedMerch.IssueDate.ToString(CultureInfo.InvariantCulture),
                MerchItemsIds = {issuedMerch.MerchItems}
            };

            return issuedMerchResponse;
        }

        public override async Task<RequestMerchResponse> RequestMerch(RequestMerchRequest request, ServerCallContext context)
        {
            var token = context.CancellationToken;
            var personId = request.PersonId;
            var merchType = new MerchItemRequestDto(request.ItemName);

            var requestedMerch = await _merchService.RequestMerch(personId, merchType, token);

            var requestedMerchResponse = new RequestMerchResponse
            {
                Id = requestedMerch.Id,
                ItemName = requestedMerch.ItemName
            };

            return requestedMerchResponse;
        }
    }
}
