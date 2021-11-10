using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OzonEdu.MerchandiseService.Infrastructure.Queries;
using OzonEdu.MerchandiseService.Models;
using OzonEdu.MerchandiseService.Services;

namespace OzonEdu.MerchandiseService.Controllers
{
    [ApiController]
    [Route("api/v1/merchandise")]
    [Produces("application/json")]
    public class MerchandiseController : ControllerBase
    {
        private readonly IMerchService _merchService;
        private readonly IMediator _mediator;

        public MerchandiseController(IMerchService merchService, IMediator mediator)
        {
            _merchService = merchService ?? throw new ArgumentNullException(nameof(merchService));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        [Route("person/{personId:long}")]
        public async Task<ActionResult<IssuedMerchInfoResponseDto>> GetIssuedMerchInfo(long personId, CancellationToken token)
        {
            var mediatrRequest = new GetIssuedMerchInfoQuery { PersonId = personId };

            var issuedMerch = await _mediator.Send(mediatrRequest, token);

            if (issuedMerch == null)
                return NotFound();

            if (issuedMerch.Count == 0)
                return Ok($"Сотруднику {personId} мерч не выдавался.");

            var issuedMerchResponse = issuedMerch.Select(im => new IssuedMerchInfoResponseDto(im)).ToList();
            
            return Ok(issuedMerchResponse);
        }

        [HttpPost]
        [Route("person/{personId:long}")]
        public async Task<ActionResult> RequestMerch(long personId, MerchItemRequestDto merchType, CancellationToken token)
        {
            var requestedMerch = await _merchService.RequestMerch(personId, merchType, token);
            if(requestedMerch == null)
                return NotFound();

            return Ok();
        }
    }
}