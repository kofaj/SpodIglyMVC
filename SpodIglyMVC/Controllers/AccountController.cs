using System;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SpodIglyMVC.App_Start;
using SpodIglyMVC.Models;
using SpodIglyMVC.ViewModels;

namespace SpodIglyMVC.Controllers
{
    public class AccountController : Controller
    {
        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            set { _userManager = value; }
        }

        private ApplicationSignInManager _signInManager;

        public ApplicationSignInManager SignInManager
        {
            get { return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>(); }
            set { _signInManager = value; }
        }
        
        // GET: Account
        // returnUrl - app zapisze strone na ktora chcial sie dostac uzytkownik i po udanym logowaniu przekieruje go na nia
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // akcja bedzie wywolana tylko i wylacznie po POST
        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModels model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                // jezeli model bedzi  e zawierac bledy to poinformuje o tym uzytkownika i zwroci mu to co wpisal
                return View(model);
            }

            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    // RedirectToLocal jest helperem aby nie przekierowac uzytkownika na zewnatrz
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("loginerror", "Nieudana próba logowania.");
                    return View(model);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, UserData = new UserData() };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            return View(model);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("", item);
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            //erquest a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallBack", "Account", new { ReturnUrl = returnUrl }));
        }

        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, create account with external provider login
                    // in reality, we might ask for providing e-mail (+ confirming it)
                    // we also need some error checking logic (ie. verification if user doesn't already exist)

                    var user = new ApplicationUser
                    {
                        UserName = loginInfo.Email,
                        Email = loginInfo.Email,
                        UserData = new UserData { Email = loginInfo.Email }
                    };

                    var registrationResult = await UserManager.CreateAsync(user);
                    if (registrationResult.Succeeded)
                    {
                        registrationResult = await UserManager.AddLoginAsync(user.Id, loginInfo.Login);
                        if (registrationResult.Succeeded)
                        {
                            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                            return RedirectToLocal(returnUrl);
                        }
                        else
                            throw new Exception("External provider association error");
                    }
                    else
                        throw new Exception("Registration error");
            }
        }


        //Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUrl) : this(provider, redirectUrl, null)
            {
            }

            public ChallengeResult(string provider, string redirectResult, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectResult;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
    }
}