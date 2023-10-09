using Banca.UtilityService.Application;
using Banca.UtilityService.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Banca.UtilityService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UtilityController : ControllerBase
    {
        private readonly IUtilityService service;

        public UtilityController(IUtilityService service)
        {
            this.service = service;
        }

        [HttpPost("GenerateOTP")]
        public async Task<IActionResult> GenerateOtp(GenerateOtpRequest request)
        {
            return Ok(await service.GenerateOtp(request));
        }

        [HttpPost("ValidateOTP")]
        public async Task<IActionResult> ValidateOtp(ValidateOtpRequest request)
        {
            return Ok(await service.ValidateOtp(request));
        }
    }

}