using System;
using System.Globalization;
using System.Threading.Tasks;
using Grpc.Core;
using MediatR;
using OzonEdu.MerchandiseService.Domain.AggregationModels.Enumerations;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchandiseService.Grpc;
using OzonEdu.MerchandiseService.Infrastructure.Commands;
using OzonEdu.MerchandiseService.Infrastructure.Queries;
using OzonEdu.MerchandiseService.Models;

namespace OzonEdu.MerchandiseService.GrpcServices
{
    public class MerchGrpcService : MerchGrpc.MerchGrpcBase
    {
        private readonly IMediator _mediator;

        public MerchGrpcService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task<GetIssuedMerchInfoResponse> GetIssuedMerchInfo(GetIssuedMerchInfoRequest request, ServerCallContext context)
        {
            var token = context.CancellationToken;
            var employeeId = request.EmployeeId;
            
            var mediatrRequest = new GetIssuedMerchInfoQuery { EmployeeId = employeeId };
            var issuedMerch = await _mediator.Send(mediatrRequest, token);
            
            if (issuedMerch == null)
                throw new RpcException(
                    new Status(StatusCode.NotFound, $"Merch info for employee (id:{employeeId}) was not found"));

            //if (issuedMerch.Count == 0)
            //    return Ok($"Сотруднику {employeeId} мерч не выдавался.");
            //var issuedMerchResponse = issuedMerch.Select(im => new IssuedMerchInfoResponseDto(im)).ToList();

            var issuedMerchResponse = new GetIssuedMerchInfoResponse
            {
                Id = issuedMerch.Id,
                EmployeeId = issuedMerch.EmployeeId,
                IssueDate = issuedMerch.IssueDate.ToString(CultureInfo.InvariantCulture),
                MerchItemsIds = {issuedMerch.MerchItems}
            };

            return issuedMerchResponse;
        }

        public override async Task<RequestMerchResponse> RequestMerch(RequestMerchRequest request, ServerCallContext context)
        {
            var token = context.CancellationToken;
            var employeeId = request.EmployeeId;
            var merchType = new MerchItemRequestDto(request.ItemName);

            var requestedMerch = await _merchService.RequestMerch(employeeId, merchType, token);

            

            //var merchTitle = new MerchPack(merchType.RequestedMerchPackType);
            //var size = Size.GetSizeFromString(merchType.Size);
            //var date = new Date(DateTime.Now);

            //var mediatrRequest = new RequestMerchCommand
            //{
            //    HRManagerId = merchType.HRManagerId,
            //    EmployeeId = employeeId,
            //    RequestedMerchPack = merchTitle,
            //    Size = size,
            //    Date = date
            //};

            //var merchRequest = await _mediator.Send(mediatrRequest, token);



            var requestedMerchResponse = new RequestMerchResponse
            {
                Id = requestedMerch.Id,
                ItemName = requestedMerch.ItemName
            };

            return requestedMerchResponse;
        }
    }
}
