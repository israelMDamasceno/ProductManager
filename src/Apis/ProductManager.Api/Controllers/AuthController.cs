using Microsoft.AspNetCore.Mvc;

namespace ProductManager.Api.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : Controller
    {
        [HttpPost]
        public IActionResult Auth(string textoGerarToken)
        {
            if (textoGerarToken == "Robbu")
            {
                var token = TokenService.GenerateToken(textoGerarToken);
                return Ok(token);
            }

            return BadRequest("username or password invalid");
        }
    }
}
