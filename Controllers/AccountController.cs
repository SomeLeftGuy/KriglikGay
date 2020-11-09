using CoursesMain.Models;
using CoursesMain.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoursesMain.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { Email = model.Email, UserName = model.Name };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action(
                        "ConfirmEmailCallback",
                        "Account",
                        new { userId = user.Id, code = code },
                        protocol: HttpContext.Request.Scheme);
                    EmailService emailService = new EmailService();
                    await emailService.SendEmailAsync(model.Email, "Confirm your account",
                        $"To confirm registration, follow the link: <a href='{callbackUrl}'>link</a>");

                    return RedirectToAction("ConfirmEmail", "Account");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmailCallback(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            await _userManager.AddToRoleAsync(user, "Admin");
            //await _userManager.AddToRoleAsync(user, "Dark");
            if (result.Succeeded)
            {
                await _userManager.SetLockoutEnabledAsync(user, false);
                return RedirectToAction("Index", "Home");
            }
            else
                return View("Error");
        }
        public IActionResult ConfirmEmail()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            LoginViewModel model = new LoginViewModel { ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList() };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByNameAsync(model.Name);
                if (user != null)
                {
                    bool emailConf = await _userManager.IsEmailConfirmedAsync(user);
                    if (emailConf)
                    {
                        bool blocked = await _userManager.GetLockoutEnabledAsync(user);
                        if (!blocked)
                        {
                            var result = await _signInManager.PasswordSignInAsync(model.Name, model.Password, model.RememberMe, false);
                            if (result.Succeeded)
                                return RedirectToAction("Index", "Home");
                            else
                                ModelState.AddModelError("", "Incorrect login or password");
                        }else
                        {
                            ModelState.AddModelError("", "You account is blocked");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Email wasn't confirm");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Incorrect login or password");
                }
            }
            return View(model);
        }

        public async Task<IActionResult> LogOff()
        {
            // удаляем аутентификационные куки
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // пользователь с данным email может отсутствовать в бд
                    // тем не менее мы выводим стандартное сообщение, чтобы скрыть 
                    // наличие или отсутствие пользователя в бд
                    return View("ForgotPasswordConfirmation");
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                EmailService emailService = new EmailService();
                await emailService.SendEmailAsync(model.Email, "Reset Password",
                    $"To reset password, follow the link: <a href='{callbackUrl}'>link</a>");
                return View("ForgotPasswordConfirmation");
            }
            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            return code == null ? View("Error") : View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return View("ResetPasswordConfirmation");
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return View("ResetPasswordConfirmation");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }
        public IActionResult SignIn(string provider, string returnUrl = null) =>
            Challenge(new AuthenticationProperties { RedirectUri = returnUrl ?? "/" }, provider);
    }
}
