using MailTask.Server.Services;
using MailTask.Shared;
using MailTask.Shared.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace MailTask.Server.Controllers
{
    public class AccountController : ControllerBase
    {
        private readonly AccountService accountService;

        public AccountController(AccountService accountService)
        {
            this.accountService = accountService;
        }

        [Authorize]
        [HttpGet(Routes.V1.UserSearch)]
        public IActionResult SearchUsers([FromQuery][Required(AllowEmptyStrings = false)] string username)
        {
            var users = accountService.SearchUsers(username);
            return Ok(users);
        }

        [Authorize]
        [HttpGet(Routes.V1.Users)]
        public IActionResult GetUsers()
        {
            var users = accountService.GetAllUsers();
            return Ok(users);
        }

        [Authorize]
        [HttpGet(Routes.V1.Users + "/{userId}")]
        public IActionResult GetUser([FromRoute][Required(AllowEmptyStrings = false)] string userId)
        {
            var foundUser = accountService.TryFindUser(userId);
            if (foundUser == null)
            {
                return NotFound(userId);
            }
            return Ok();
        }

        [HttpGet(Routes.V1.CurrentUser)]
        public IActionResult Me()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var user = new User()
                {
                    Id = User.Claims.First(t => t.Type == ClaimTypes.NameIdentifier).Value,
                    Username = User.Identity.Name
                };
                return Ok(user);
            }
            return Unauthorized();
        }

        [HttpPost(Routes.V1.Login)]
        public async Task<IActionResult> Login([FromBody][Required] string username)
        {
            var user = accountService.GetUser(username);
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };
            var claimIdentity = new ClaimsIdentity(claims, "MailUser");
            var claimPrincipal = new ClaimsPrincipal(claimIdentity);
            await HttpContext.SignInAsync("MailUser", claimPrincipal);
            return Ok();
        }

        [Authorize]
        [HttpPost(Routes.V1.Logout)]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(User.Identity.AuthenticationType);
            return Ok();
        }
    }
}