using System;
using System.Threading.Tasks;
using ObjectContainer.Objects;
using Microsoft.AspNetCore.Mvc;
using BL_BusinessLogic_.Extensions;
using BL_BusinessLogic_;
using Microsoft.Extensions.Logging;
using WebView.Loggers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using Microsoft.Extensions.Caching.Memory;

namespace WebView.Controllers
{
    public class AccountController : Controller
    {
        private readonly ManagerBLL _serviceManager;
        private readonly ContactBLL _serviceContact;
        private readonly ILogger<AccountController> _logger;
        private readonly IMemoryCache _cache;

        public AccountController(ManagerBLL service, ILogger<AccountController> logger, ContactBLL serviceContact, IMemoryCache cache)
        {
            _serviceManager = service;
            _logger = logger;
            _serviceContact = serviceContact;
            _cache = cache;
        }

        public string Secret()
        {
            return "успешный вход";
        }

        public IActionResult login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> login(Manager manager)
        {

            _logger.LogInformation(LogEvents.GetItem, "Get item - {manager}", manager);
            var res = await _serviceManager.GetById(manager.id);
            if (res != null)
            {
                await Authenticate(manager.Email);
                return RedirectToAction("Secret");
            }
            ModelState.AddModelError("", "не существует такого пользователя");
            return View();
        }


        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Manager manager)
        {
            try
            {
                
                if (manager == null)
                    return NotFound();
                var res = _serviceManager.GetById(manager.id);
                if (res == null)
                {
                    _logger.LogDebug("Login take null from view");
                    await Task.Run(async () => { await _serviceManager.Add(manager); });
                    await Authenticate(manager.Email);
                    await Task.Run(() => { _cache.Remove("managers"); });
                    return RedirectToAction("Details", "Contact");
                }
                return View();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "____EROR___{e}", e.Message);
                return NotFound();
            }
        }
        ////////////////////////////////////////////////////////Log and reg for Contacts
        
        public IActionResult loginContact()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> loginContact(Contact contact)
        {
            _logger.LogInformation(LogEvents.GetItem, "Get item - {contact}", contact);
            var res = _serviceContact.GetById(contact.id);
            if (res != null)
            {
                await Authenticate(contact.Email);
                return RedirectToAction("GetById", "Manager", $"?id={contact.id}");
            }
            ModelState.AddModelError("", "не существует такого пользователя");
            return View();
        }
        public IActionResult registerContact()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> registerContact(Contact contact)
        {
            try
            {

                if (contact == null)
                    return NotFound();
                var res = await _serviceManager.GetById(contact.id);
                if (res == null)
                {
                    _logger.LogDebug("Login take null from view");
                    contact.Visible = true;
                    await Task.Run(() => {_serviceContact.Add(contact); });
                    await Task.Run(() => { _cache.Remove("contacts"); });
                    await Authenticate(contact.Email);
                    return RedirectToAction("GetById", "Contacts");
                }
                return View();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "____EROR___{e}", e.Message);
                return NotFound();
            }
        }



        private async Task Authenticate(string userName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            ClaimsIdentity identityData = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identityData));
        }

        private async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("login");
        }
    }
}


