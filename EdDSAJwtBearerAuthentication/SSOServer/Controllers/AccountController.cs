using EdDSAJwtBearer;
using Microsoft.AspNetCore.Mvc;
using SSOServer.Models;
using SSOServer.ViewModels;
using System.Security.Claims;

namespace SSOServer.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        EdDSAJwtBearerServer Server;

        public AccountController(EdDSAJwtBearerServer server)
        {
            Server = server;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserCredentials credentials)
        {
            IActionResult Response = Unauthorized();

            var User = Repository.GetUser(credentials.Email, credentials.Password);

            if (User != null)
            {
                string Token = CreateToken(Server, User);
                Response = Ok(Token);
            }

            return Response;
        }

        private string CreateToken(EdDSAJwtBearerServer server, User user)
        {
            var Claims = new[]
            {
                new Claim("sub", user.Id.ToString()),
                new Claim("firstName", user.FirstName),
                new Claim("lastName", user.LastName),
                new Claim("email", user.Email)
            }.ToList();

            return server.CreateToken(Claims, user.Roles, DateTime.Now.AddMinutes(30));
        }
    }
}
