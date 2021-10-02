using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Basics.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }

        public IActionResult Authenticate()
        {
            var gerryClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "Bob"),
                new Claim(ClaimTypes.Email, "Bob@gerry.com"),
                new Claim("Gerry.Says", "Very nice boi."),
            };
            var licenseClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "Bob K Foo"),
                new Claim("Driving.License", "A1"),
            };
            
            var gerryIdentity = new ClaimsIdentity(gerryClaims, "Gerry Identity");
            var licenseIdentity = new ClaimsIdentity(licenseClaims, "Government");
            
            var userPrincipal = new ClaimsPrincipal(new[] { gerryIdentity,licenseIdentity });

            HttpContext.SignInAsync(userPrincipal);
            return RedirectToAction("Index");
        }
    }
}