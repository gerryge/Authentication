using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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

        [Authorize(Policy = "Claim.DoB")]
        public IActionResult SecretPolicy()
        {
            return View("Secret");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult SecretRole()
        {
            return View("Secret");
        }

        public IActionResult Authenticate()
        {
            var gerryClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "Bob", ClaimValueTypes.String),
                new Claim(ClaimTypes.Email, "Bob@gerry.com"),
                new Claim(ClaimTypes.DateOfBirth, "1/1/1990"),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim("Gerry.Says", "Very nice boi."),
            };
            var licenseClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "Bob K Foo"),
                new Claim("Driving.License", "A1"),
            };

            var gerryIdentity = new ClaimsIdentity(gerryClaims, "Gerry Identity");
            var licenseIdentity = new ClaimsIdentity(licenseClaims, "Government");

            var userPrincipal = new ClaimsPrincipal(new[] { gerryIdentity, licenseIdentity });
            //Debug.Assert(userPrincipal.Identities.First() is WindowsIdentity,"IIdentity is not WindowsIdentity");
            //IAuthenticationHandler
            //IAuthenticationSignInHandler
            //CookieAuthenticationHandler
            //IAuthenticationRequestHandler
            //IAuthenticationHandlerProvider提供的GetHandlerAsync方法，根据当前HttpContext上下文和认证方案名来提供对用的IAuthenticaionHandler
            
            //AuthenticationScheme的HandleType为我们提供所需的认证处理器类型，
            //但是接下来的问题是如何根据认证方案名称得到对应的AuthenticationScheme对象，
            //这个问题需要借助IAuthenticationSchemeProvider对象来解决，
            //它不仅能够通过GetSchemeAsync(string name)帮助我们获取所需的方案，还能通过AddScheme注册方案
            HttpContext.SignInAsync(userPrincipal);
            return RedirectToAction("Index");
        }
    }
}