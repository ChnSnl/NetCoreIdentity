using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetCoreIdentity.Models;
using NetCoreIdentity.Models.Entities;
using NetCoreIdentity.Models.ViewModels.AppUsers.PureVms;
using System.Diagnostics;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace NetCoreIdentity.Controllers
{
    [AutoValidateAntiforgeryToken] //Get ile gelen sayfada verilen �zel bir token sayesinde post'un bu tokens�z yap�lmamas�n� sa�lar.
    public class HomeController : Controller
    {
        //Identity i�erisinde bulunan Manager Class'lar bize Identity yap�lar�m�zda crud ve businesslogic gibi i�lemler yapmam�z� sa�lar.
        private readonly ILogger<HomeController> _logger;

        readonly UserManager<AppUser> _userManager;
        readonly RoleManager<AppRole> _roleManager;
        readonly SignInManager<AppUser> _signInManager;

        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager)
        {
            _logger = logger;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Register(UserRegisterRequestModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = new()
                {
                    UserName = model.UserName,
                    Email = model.Email
                };

                IdentityResult result = await _userManager.CreateAsync(appUser, model.Password);

                if (result.Succeeded)
                {
                    //AppRole role = await _roleManager.FindByNameAsync("Admin");
                    //if (role == null) await _roleManager.CreateAsync(new() { Name = "Admin" });
                    //await _userManager.AddToRoleAsync(appUser, "Admin");

                    AppRole role = await _roleManager.FindByNameAsync("Member");
                    if (role == null) await _roleManager.CreateAsync(new() { Name = "Member" });
                    await _userManager.AddToRoleAsync(appUser, "Member");

                    return RedirectToAction("Register");
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }
            return View(model);
        }

        public IActionResult SignIn(string returnUrl)
        {
            UserSignInRequestModel usModel = new()
            {
                ReturnUrl = returnUrl
            };

            return View(usModel);
        }

        [HttpPost]

        public async Task<IActionResult> SignIn(UserSignInRequestModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = await _userManager.FindByNameAsync(model.UserName);
                SignInResult result = await _signInManager.PasswordSignInAsync(appUser, model.Password, model.RememberMe, true);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrWhiteSpace(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }

                    IList<string> roles = await _userManager.GetRolesAsync(appUser);

                    if (roles.Contains("Admin"))
                    {
                        return RedirectToAction("AdminPanel");
                    }
                    else if (roles.Contains("Member"))
                    {
                        return RedirectToAction("MemberPanel");
                    }

                    return RedirectToAction("Panel");
                }

                else if (result.IsLockedOut)
                {
                    DateTimeOffset? lockOutEndDate = await _userManager.GetLockoutEndDateAsync(appUser);

                    ModelState.AddModelError("", $"Hesab�n�z {(lockOutEndDate.Value.UtcDateTime - DateTime.UtcNow).Minutes} dakika s�reyle ask�ya al�nm��t�r.");
                }
                else
                {
                    string message = "";
                    if (appUser != null)
                    {
                        int maxFailedAttempts = _userManager.Options.Lockout.MaxFailedAccessAttempts;
                        message = $"E�er {maxFailedAttempts - await _userManager.GetAccessFailedCountAsync(appUser)} kez daha hatal� giri� yaparsan�z hesab�n�z ask�ya al�nacakt�r.";
                    }
                    else
                    {
                        message = "Kullan�c� bulunamad�";
                    }
                    ModelState.AddModelError("", message);
                }
            }
            return View(model);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return View("Index");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AdminPanel()
        {
            return View();
        }

        [Authorize(Roles = "Member")]
        public IActionResult MemberPanel()
        {
            return View();
        }

        public IActionResult Panel()
        {
            return View();
        }

    }

}
