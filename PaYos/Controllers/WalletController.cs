using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PaYos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        public WalletController()
        {
        }

        [HttpGet("payReturn")]
        public IActionResult payReturn()
        {
            return Ok("Hello World");
        }
    }
}
