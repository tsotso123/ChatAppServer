using ChatAppServer.Data.Repositories;
using ChatAppServer.Utils;
using Microsoft.AspNetCore.Mvc;

namespace ChatAppServer.Controllers
{
    [Route("/[controller]")]
    public class AccountController : Controller
    {
        public IAccountRepository accountRepository { get; set; }
        private JwtHelper jwtHelper { get; set; }
        public AccountController(IAccountRepository accountRepository, JwtHelper jwtHelper)
        {
            this.accountRepository = accountRepository;
            this.jwtHelper = jwtHelper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("/[controller]/SearchUsers")] // Explicit route for the SearchUsers method
        public async Task<IActionResult> SearchUsers(string usernameQuery)
        {
            var matching = await accountRepository.GetAccountsMatchingQuery(usernameQuery);
            return Ok(matching);
        }

        [HttpPost]
        [Route("/[controller]/Login")] // Explicit route for the SearchUsers method
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var username = loginRequest.Username;
            var password = loginRequest.Password;

            if (username == null || password == null)
            {
                return BadRequest();
            }

            string storedHashedPassword = await accountRepository.getPasswordForUsername(username);
            if (!BCrypt.Net.BCrypt.Verify(password, storedHashedPassword))
            {
                return Unauthorized("Invalid credentials");
            }

            var token = jwtHelper.GenerateToken(username);
            
            HttpContext.Response.Headers["authToken"] = token;


            // Return success response without including the token in the JSON
            return Ok(new { message = "Login successful"});

        }

        public class LoginRequest
        {
            public string? Username { get; set; }
            public string? Password { get; set; }
        }
    }
}
