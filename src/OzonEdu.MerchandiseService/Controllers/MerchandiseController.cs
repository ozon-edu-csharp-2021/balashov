using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OzonEdu.MerchandiseService.Domain.AggregationModels.Enumerations;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchAggregate;
using OzonEdu.MerchandiseService.Domain.AggregationModels.ValueObjects;
using OzonEdu.MerchandiseService.Infrastructure.Commands;
using OzonEdu.MerchandiseService.Infrastructure.Queries;
using OzonEdu.MerchandiseService.Models;

namespace OzonEdu.MerchandiseService.Controllers
{
    [ApiController]
    [Route("api/v1/merchandise")]
    [Produces("application/json")]
    public class MerchandiseController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MerchandiseController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        [Route("GetIssuedMerchInfo")]
        public async Task<ActionResult<MerchandiseRequestResponseDto>> GetIssuedMerchInfo(long employeeId, CancellationToken token)
        {
            var mediatrRequest = new GetIssuedMerchInfoQuery { EmployeeId = employeeId };

            var issuedMerch = await _mediator.Send(mediatrRequest, token);

            if (issuedMerch == null)
                return NotFound();

            if (issuedMerch.Count == 0)
                return Ok($"Сотруднику {employeeId} мерч не выдавался.");

            var issuedMerchResponse = issuedMerch.Select(im => new MerchandiseRequestResponseDto(im)).ToList();
            
            return Ok(issuedMerchResponse);
        }

        [HttpGet]
        [Route("GetIssuingMerchInfo")]
        public async Task<ActionResult<MerchandiseRequestResponseDto>> GetIssuingMerchInfo(long employeeId, CancellationToken token)
        {
            var mediatrRequest = new GetIssuingMerchInfoQuery { EmployeeId = employeeId };

            var issuingMerch = await _mediator.Send(mediatrRequest, token);

            if (issuingMerch == null)
                return NotFound();

            if (issuingMerch.Count == 0)
                return Ok($"Сотрудник {employeeId} мерч не ожидает.");

            var issuedMerchResponse = issuingMerch.Select(im => new MerchandiseRequestResponseDto(im)).ToList();

            return Ok(issuedMerchResponse);
        }

        [HttpPost]
        public async Task<ActionResult> RequestMerch(MerchandiseRequestRequestDto request, CancellationToken token)
        {
            var merchTitle = new MerchPack(request.RequestedMerchPackType);
            var size = Size.GetSizeFromString(request.Size);
            var date = new Date(DateTime.Now);

            var mediatrCommand = new RequestMerchCommand
            {
                HRManagerId = request.HRManagerId,
                EmployeeId = request.EmployeeId,
                RequestedMerchPack = merchTitle,
                Size = size,
                Date = date
            };

            MerchandiseRequest merchRequest = await _mediator.Send(mediatrCommand, token);

            var responseDto = new MerchandiseRequestResponseDto(merchRequest);
            return Ok(responseDto);
        }

        [HttpPost]
        [Route("DoneMerchRequest")]
        public async Task<ActionResult> MerchandiseRequestDone(long merchandiseRequestId, CancellationToken token)
        {
            var mediatrCommand = new MerchRequestDoneCommand
            {
                MerchRequestId = merchandiseRequestId
            };

            MerchandiseRequest merchRequest = await _mediator.Send(mediatrCommand, token);

            var responseDto = new MerchandiseRequestResponseDto(merchRequest);
            return Ok(responseDto);
        }
    }
}