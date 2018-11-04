using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SpodIglyMVC.App_Start;
using SpodIglyMVC.DAL;
using SpodIglyMVC.Models;
using SpodIglyMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SpodIglyMVC.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        StoreContext db = new StoreContext();

      //  private IMailService mailService;

        public ManageController(StoreContext context/*, IMailService mailservice*/)
        {
            this.db = context;
            //this.mailService = mailService;
        }

        public ManageController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            LinkSuccess,
            Error
        }

        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            set { _userManager = value; }
        }

        private IAuthenticationManager _AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
                return user.PasswordHash != null;

            return false;
        }

        // GET: Manage
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            if (TempData["Viewdata"] != null)
                ViewData = (ViewDataDictionary)TempData["Viewdata"];


            if (User.IsInRole("Admin"))
                ViewBag.UserIsAdmin = true;
            else
                ViewBag.UserIsAdmin = false;

            var user = UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
                return View("Error");

            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
           // var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth));

            var model = new ManageCredentialsViewModel
            {
                Message = message,
                HasPassword = this.HasPassword(),
                CurrentLogin = userLogins,
               // OtherLogins = otherLogins,
                //ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1,
                //UserData = user.UserData
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // Bind jest tutaj uzyty aby Razor mogl prawidlowo bindowac model w widoku. Wynika to z faktu mocnego zagniezdzenia danych (nie odwolujemy sie bezposrednio do wlasciwosci)
        public async Task<ActionResult> ChangeProfile([Bind(Prefix = "UserData")]UserData userData)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                user.UserData = userData;
                var result = await UserManager.UpdateAsync(user);

               // AddErrors(result);
            }

            if (!ModelState.IsValid)
            {
                TempData["ViewData"] = ViewData;
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Changepassword([Bind(Prefix = "ChangePasswordViewModel")]ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ViewData"] = ViewData;
                return RedirectToAction("Index");
            }

            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                   // await SignInAsync(user, isPersistent: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }

          //  AddErrors(result);

            //in case we have login errors
            if (!ModelState.IsValid)
            {
                TempData["ViewData"] = ViewData;
                return RedirectToAction("Index");
            }

            var message = ManageMessageId.ChangePasswordSuccess;
            return RedirectToAction("Index", new { Message = message });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallBack", "Manage"), User.Identity.GetUserId());
        }

        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        //public async Task<ActionResult> LinkLoginCallBack()
        //{
        //    var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
        //    if (loginInfo != null)
        //        return RedirectToAction("Index", new { Message = ManageMessageId.Error });

        //    var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
        //    return result.Succeeded ? RedirectToAction("Index", new { Message = ManageMessageId.LinkSuccess }) : RedirectToAction("Index", new { Message = ManageMessageId.Error });
        //}

    }
}