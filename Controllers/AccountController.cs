/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Portal.Models;
using Portal.ViewModels;
using Portal.Services;

namespace Portal.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private SportDataContext db = new SportDataContext();
        private UserStore<User> store;
        public AccountController()            
        {
            UserManager = new UserManager<User>(new UserStore<User>(db));
            UserManager.UserValidator = new UserValidator<User>(UserManager) { AllowOnlyAlphanumericUserNames = false };
        }
        
        public AccountController(UserManager<User> userManager)
        {
            UserManager = userManager;
        }

        public UserManager<User> UserManager { get; private set; }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [AllowAnonymous]
        public ActionResult LogOn(string returnUrl)
        {
            return RedirectToActionPermanent("Login", new { returnUrl = returnUrl });
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindAsync(model.UserName, model.Password);
                if (user != null)
                {
                    await SignInAsync(user, model.RememberMe);
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model, string returnUrl)
        {
            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Пароль и Подтверждение пароля должны совпадать");
            }
            if (ModelState.IsValid)
            {
                User user = null;
                if (!string.IsNullOrWhiteSpace(model.DriverLicense))
                {
                    user = db.Users.FirstOrDefault(u => u.Licenses.Any(n => n.Number == model.DriverLicense && n.Type == LicenseType.Driver) && u.PasswordHash == null);
                }
                if (user==null && !string.IsNullOrWhiteSpace(model.MarchalLicense))
                {
                    user = db.Users.FirstOrDefault(u => u.Licenses.Any(n => n.Number == model.MarchalLicense && n.Type==LicenseType.Marchal) && u.PasswordHash == null);
                }

                if (user == null)
                {
                    user = new User()
                    {
                        UserName = model.UserName,
                        Address = model.Address,
                        BirthDate = model.BirthDate,
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Location = model.Location,
                        Passport = model.Passport,
                        Phone = model.Phone,
                        Licenses = new List<License>()
                    };

                    if (model.DriverLicense != null)
                    {
                        user.Licenses.Add(new License()
                        {
                            Number = model.DriverLicense,
                            Season = 2014,
                            IssuesOn = DateTime.Today
                        });
                    }

                    if (model.MarchalLicense != null)
                    {
                        user.Licenses.Add(new License()
                        {
                            Number = model.MarchalLicense,
                            Season = 2014,
                            IssuesOn = DateTime.Today,
                            Type = LicenseType.Marchal
                        });
                    }


                    var result = await UserManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        await SignInAsync(user, isPersistent: false);
                        if (string.IsNullOrWhiteSpace(returnUrl)) return Redirect("~/");
                        else return Redirect(returnUrl);
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
                else
                {
                    user.UserName = model.UserName;
                    await db.SaveChangesAsync();
                    var result = await UserManager.AddPasswordAsync(model.UserName, model.Password);
                    if (result.Succeeded)
                    {
                        await SignInAsync(user, isPersistent: false);
                        if (string.IsNullOrWhiteSpace(returnUrl)) return Redirect("~/");
                        else return Redirect(returnUrl);
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            ManageMessageId? message = null;
            IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage
        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Ваш пароль был изменен."
                : message == ManageMessageId.SetPasswordSuccess ? "Ваш пароль был установлен."
                : message == ManageMessageId.RemoveLoginSuccess ? "Внешний сервис идентификации удален."
                : message == ManageMessageId.Error ? "Произошла ошибка при выполнении действия."
                : message == ManageMessageId.ProfileUpdated ? "Профиль обновлен."
                : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            bool hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var user = await UserManager.FindAsync(loginInfo.Login);
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // If the user does not have an account, then prompt the user to create an account
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { UserName = loginInfo.DefaultUserName, LoginProvider = loginInfo.Login.LoginProvider });
            }
        }

        //
        // POST: /Account/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
        }

        //
        // GET: /Account/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            if (result.Succeeded)
            {
                return RedirectToAction("Manage");
            }
            return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new User() {
                    UserName = model.UserName,
                    Address = model.Address,
                    BirthDate = model.BirthDate,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Location = model.Location,
                    Passport = model.Passport,
                    Phone = model.Phone,
                    Licenses = new List<License>()
                };                

                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInAsync(user, isPersistent: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff        
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return Redirect("/");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult RemoveAccountList()
        {
            var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
        }

        [ChildActionOnly]
        public ActionResult EditProfile()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());

            db.Entry(user).Collection(u => u.Licenses).Load();
            var license = user.Licenses.OrderByDescending(l=>l.IssuesOn).FirstOrDefault(l=>l.Type==LicenseType.Driver && l.Season==2014);
            ViewBag.DriverLicense = license != null ? license.Number : "";
            license = user.Licenses.OrderByDescending(l => l.IssuesOn).FirstOrDefault(l => l.Type == LicenseType.Marchal && l.Season == 2014);
            ViewBag.MarchalLicense = license != null ? license.Number : "";

            return PartialView("_EditProfile", user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveProfile([Bind(Exclude = "PasswordHash,SecurityStamp,UserName,Roles,Logins,Claims,Id")]User user, string driverLicense, string marchalLicense)
        {
            var currentUser = UserManager.FindById(User.Identity.GetUserId());
            db.Entry(currentUser).Collection(u => u.Licenses).Load();

            currentUser.LastName = user.LastName;
            currentUser.Address = user.Address;
            currentUser.BirthDate = user.BirthDate;
            currentUser.Email = user.Email;
            currentUser.FirstName = user.FirstName;
            currentUser.Location = user.Location;
            currentUser.Passport = user.Passport;
            currentUser.Phone = user.Phone;

            if (!string.IsNullOrWhiteSpace(driverLicense) && !currentUser.Licenses.Any(l=>l.Number==driverLicense && l.Type== LicenseType.Driver && l.Season==2014))
            {
                currentUser.Licenses.Add(new License()
                {
                    Season = 2014,
                    Number = driverLicense,
                    IssuesOn = DateTime.Today,
                    Type= LicenseType.Driver
                });
            }

            if (!string.IsNullOrWhiteSpace(marchalLicense) && !currentUser.Licenses.Any(l => l.Number == marchalLicense && l.Type == LicenseType.Marchal && l.Season == 2014))
            {
                currentUser.Licenses.Add(new License()
                {
                    Season = 2014,
                    Number = marchalLicense,
                    IssuesOn = DateTime.Today,
                    Type = LicenseType.Marchal
                });
            }

            db.SaveChanges();
            return RedirectToAction("Manage", new { message = ManageMessageId.ProfileUpdated });
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> ResetPassword(string email)
        {
            using (var ctx = new SportDataContext())
            {
                var user = ctx.Users.FirstOrDefault(u => u.Email == email);

                if (user != null)
                {
                    var pwd = CreateRandomPassword(8);

                    await UserManager.RemovePasswordAsync(user.Id);
                    await UserManager.AddPasswordAsync(user.Id, pwd);

                    EmailService.SendPasswordResetMail(user, pwd);
                    return View(true);
                }
                else
                    return View(false);
            }
        }

        private static string CreateRandomPassword(int passwordLength)
        {
            string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?_-";
            char[] chars = new char[passwordLength];
            Random rd = new Random();

            for (int i = 0; i < passwordLength; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(User user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error,
            ProfileUpdated
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return Redirect("~/");
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion


    }
}