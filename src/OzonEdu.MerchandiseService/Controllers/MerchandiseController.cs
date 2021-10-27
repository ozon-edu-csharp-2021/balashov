using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
        private readonly IMerchService _merchService;

        public MerchandiseController(IMerchService merchService)
        {
            _merchService = merchService;
        }

        [HttpGet]
        [Route("person/{personId:long}")]
        [ProducesResponseType(typeof(IssuedMerchInfoResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetIssuedMerchInfo(long personId, CancellationToken token)
        {
            var issuedMerch = await _merchService.GetIssuedMerchInfo(personId, token);
            if (issuedMerch == null)
                return NotFound();

            var issuedMerchResponse = new IssuedMerchInfoResponseDto(issuedMerch);
            return Ok(issuedMerchResponse);

        }

        [HttpPost]
        [Route("person/{personId:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RequestMerch(long personId, MerchItemRequestDto merchType, CancellationToken token)
        {
            var requestedMerch = await _merchService.RequestMerch(personId, merchType, token);
            if(requestedMerch == null)
                return NotFound();

            return Ok();
        }
    }
}