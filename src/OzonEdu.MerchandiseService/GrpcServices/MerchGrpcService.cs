using System;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using MediatR;
using OzonEdu.MerchandiseService.Domain.AggregationModels.Enumerations;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchandiseService.Grpc;
using OzonEdu.MerchandiseService.Infrastructure.Commands;
using OzonEdu.MerchandiseService.Infrastructure.Queries;

namespace OzonEdu.MerchandiseService.GrpcServices
{
    public class MerchGrpcService : MerchGrpc.MerchGrpcBase
    {
        private readonly IMediator _mediator;

        public MerchGrpcService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task<GetIssuedMerchInfoResponse> GetIssuedMerchInfo(GetIssuedMerchInfoRequest request,
            ServerCallContext context)
        {
            var token = context.CancellationToken;
            var employeeId = request.EmployeeId;

            var mediatrRequest = new GetIssuedMerchInfoQuery { EmployeeId = employeeId };
            var issuedMerch = await _mediator.Send(mediatrRequest, token);

            if (issuedMerch == null)
                throw new RpcException(
                    new Status(StatusCode.NotFound, $"Merch info for employee (id:{employeeId}) was not found"));

            var issuedMerchResponse = new GetIssuedMerchInfoResponse()
            {
                IssuedMerch =
                {
                    issuedMerch.Select(im => new RequestMerchResponse()
                    {
                        Id = im.Id,
                        Status = im.Status.ToString(),
                        HrManagerId = im.HRManagerId,
                        HrManagerContactPhone = im.HRManagerContactPhone.Phone,
                        EmployeeId = im.EmployeeId,
                        EmployeeContactPhone = im.EmployeeContactPhone.Phone,
                        Size = im.Size.Name,
                        RequestedMerchPackId = im.RequestedMerchPack.PackTitle.Id

                    })
                }
            };

            return issuedMerchResponse;
        }

        public override async Task<RequestMerchResponse> RequestMerch(RequestMerchRequest request, ServerCallContext context)
        {
            var token = context.CancellationToken;

            var merchTitle = new MerchPack(request.MerchPackTitle);
            var size = Size.GetSizeFromString(request.Size);
            var date = new Date(DateTime.Now);

            var mediatrRequest = new RequestMerchCommand
            {
                HRManagerId = -1, //any free manager
                EmployeeId = request.EmployeeId,
                RequestedMerchPack = merchTitle,
                Size = size,
                Date = date
            };

            var merchRequest = await _mediator.Send(mediatrRequest, token);

            var requestedMerchResponse = new RequestMerchResponse()
            {
                Id = merchRequest.Id,
                Status = merchRequest.Status.ToString(),
                HrManagerId = merchRequest.HRManagerId,
                HrManagerContactPhone = merchRequest.HRManagerContactPhone.Phone,
                EmployeeId = merchRequest.EmployeeId,
                EmployeeContactPhone = merchRequest.EmployeeContactPhone.Phone,
                Size = merchRequest.Size.Name,
                RequestedMerchPackId = merchRequest.RequestedMerchPack.PackTitle.Id

            };

            return requestedMerchResponse;
        }
    }
}
