using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OzonEdu.MerchandiseService.Models;
using OzonEdu.MerchandiseService.Services;

namespace OzonEdu.MerchandiseService.Controllers
{
    [ApiController]
    [Route("api/v1/merchandise")]
    [Produces("application/json")]
    public class MerchandiseController : ControllerBase
    {
        private readonly IMerchandiseService _merchService;

        public MerchandiseController(IMerchandiseService stockService)
        {
            _merchService = stockService;
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
        
        [HttpGet]
        [Route("person/{personId:long}")]
        public async Task<ActionResult<IssuingMerchResponseDto>> GetMerchInfo(long personId, CancellationToken token)
        {
            var issuedMerch = await _merchService.GetMerchIssueInfo(personId, token);
            if(issuedMerch == null)
                return NotFound();

            var issuedMerchResponse = new IssuingMerchResponseDto(issuedMerch);
            return Ok(issuedMerchResponse);

        }
    }
}