using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using MediatR;
using OzonEdu.MerchandiseService.Domain.AggregationModels.Enumerations;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate;
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

        public override async Task<GetMerchInfoResponse> GetIssuedMerchInfo(GetMerchInfoRequest request,
            ServerCallContext context)
        {
            return await GetMerchInfo(request, context, true);
        }

        public override async Task<GetMerchInfoResponse> GetIssuingMerchInfo(GetMerchInfoRequest request,
            ServerCallContext context)
        {
            return await GetMerchInfo(request, context, false);
        }

        private async Task<GetMerchInfoResponse> GetMerchInfo(GetMerchInfoRequest request, ServerCallContext context, bool isDoneMerch)
        {
            var token = context.CancellationToken;
            var employeeId = request.EmployeeId;

            var merch = new List<MerchandiseRequest>();
            if (isDoneMerch)
            {
                var mediatrRequest = new GetIssuedMerchInfoQuery { EmployeeId = employeeId };
                merch = await _mediator.Send(mediatrRequest, token);
            }
            else
            {
                var mediatrRequest = new GetIssuingMerchInfoQuery { EmployeeId = employeeId };
                merch = await _mediator.Send(mediatrRequest, token);
            }

            if (merch == null || merch.Count == 0)
                throw new RpcException(
                    new Status(StatusCode.NotFound, $"Merch info for employee (id:{employeeId}) was not found"));

            var issuingMerchResponse = new GetMerchInfoResponse()
            {
                MerchRequest =
                {
                    merch.Select(im => new RequestMerchResponse()
                    {
                        Id = im.Id,
                        Status = im.Status.ToString(),
                        HrManagerId = im.HRManagerId,
                        EmployeeEmail = im.EmployeeEmail.EmailString,
                        Size = im.Size.Name,
                        RequestedMerchPackId = im.RequestedMerchPack.PackTitle.Id
                    })
                }
            };

            return issuingMerchResponse;
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
                EmployeeEmail = new Email(request.EmployeeEmail),
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
                EmployeeEmail = merchRequest.EmployeeEmail.EmailString,
                Size = merchRequest.Size.Name,
                RequestedMerchPackId = merchRequest.RequestedMerchPack.PackTitle.Id

            };

            return requestedMerchResponse;
        }

        public override async Task<RequestMerchResponse> MerchRequestDone(MerchRequestDoneRequest request, ServerCallContext context)
        {
            var token = context.CancellationToken;

            var mediatrRequest = new MerchRequestDoneCommand { MerchRequestId = request.MerchRequestId };

            var merchRequest = await _mediator.Send(mediatrRequest, token);

            var requestedMerchResponse = new RequestMerchResponse()
            {
                Id = merchRequest.Id,
                Status = merchRequest.Status.ToString(),
                HrManagerId = merchRequest.HRManagerId,
                EmployeeEmail = merchRequest.EmployeeEmail.EmailString,
                Size = merchRequest.Size.Name,
                RequestedMerchPackId = merchRequest.RequestedMerchPack.PackTitle.Id

            };

            return requestedMerchResponse;
        }
    }
}
