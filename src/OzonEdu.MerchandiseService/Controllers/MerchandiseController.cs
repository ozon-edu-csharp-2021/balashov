﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OzonEdu.MerchandiseService.Domain.AggregationModels.Enumerations;
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
        [Route("employee/{employeeId:long}")]
        public async Task<ActionResult<IssuedMerchInfoResponseDto>> GetIssuedMerchInfo(long employeeId, CancellationToken token)
        {
            var mediatrRequest = new GetIssuedMerchInfoQuery { EmployeeId = employeeId };

            var issuedMerch = await _mediator.Send(mediatrRequest, token);

            if (issuedMerch == null)
                return NotFound();

            if (issuedMerch.Count == 0)
                return Ok($"Сотруднику {employeeId} мерч не выдавался.");

            var issuedMerchResponse = issuedMerch.Select(im => new IssuedMerchInfoResponseDto(im)).ToList();
            
            return Ok(issuedMerchResponse);
        }

        [HttpPost]
        [Route("employee/{employeeId:long}")]
        public async Task<ActionResult> RequestMerch(long employeeId, MerchItemRequestDto merchType, CancellationToken token)
        {
            var merchTitle = new MerchPack(merchType.RequestedMerchPackType);
            var size = Size.GetSizeFromString(merchType.Size);
            var date = new Date(DateTime.Now);

            var mediatrRequest = new RequestMerchCommand
            {
                HRManagerId = merchType.HRManagerId,
                EmployeeId = employeeId,
                RequestedMerchPack = merchTitle,
                Size = size,
                Date = date
            };

            var merchRequest = await _mediator.Send(mediatrRequest, token);

            return Ok(merchRequest);
        }
    }
}