using ChatAppServer.Data.Repositories;
using ChatAppServer.Models;
using ChatAppServer.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ChatAppServer.Controllers
{

    [Route("/[controller]")]
    public class ChatManagerController : Controller
    {

        public IAccountRepository accountRepository { get; set; }
        public ChatManagerController(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        [Authorize(Roles = "LoggedInUser")]
        [HttpPost]
        public async Task<IActionResult> Index()
        {
            var username = User.Identity?.Name;
            var dcList = await accountRepository.GetDirectChats(username!,1,5);
            return View(dcList);
        }

        [Authorize(Roles = "LoggedInUser")]
        [HttpPost]
        [Route("/[controller]/GetMessages")] // Explicit route for the SearchUsers method
        public async Task<IActionResult> GetMessages(string user)
        {
            var username = User.Identity?.Name;
            var account1 = await accountRepository.GetAccountByUsername(username!);
            var account2 = await accountRepository.GetAccountByUsername(user);

            var dc = accountRepository.GetDirectChat(account1!,account2!);
            var msgs = dc?.Messages; // not ok to get all msgs
            return Ok(msgs);
        }
    }
}
