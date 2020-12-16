using System;
using System.Threading.Tasks;
using ObjectContainer.Objects;
using Microsoft.AspNetCore.Mvc;
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
using Microsoft.AspNetCore.Authorization;
using WebView.ViewModels;

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
                await Authenticate(manager);
                return RedirectToAction("TestForManager", "Main");
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
                manager.id = Guid.NewGuid();
                _logger.LogDebug("Login take null from view");
                await Task.Run(async () => { await _serviceManager.Add(manager); });
                await Authenticate(manager);
                await Task.Run(() => { _cache.Remove("managers"); });
                return RedirectToAction("Details", "Contact");

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
            try
            {
                _logger.LogInformation(LogEvents.GetItem, "Get item - {contact}", contact.id);
                var res = _serviceContact.GetById(contact.id);
                if (res != null)
                {
                    await Authenticate(contact);
                    return RedirectToAction("TestForContact", "Main");
                }
                ModelState.AddModelError("", "не нашлось такого пользователя");
                return View();
            }
            catch (Exception e)
            {
                _logger.LogError("{e}", e.Message);
                return RedirectToAction("Index", "Main");
            }

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
                {
                    return View();
                    _logger.LogDebug("Login take null from view");
                }
                contact.id = Guid.NewGuid();


                contact.Visible = true;
                await Task.Run(() => { _serviceContact.Add(contact); });
                await Task.Run(() => { _cache.Remove("contacts"); });
                await Authenticate(contact);
                return RedirectToAction("GetById", "Contacts");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "____EROR___{e}", e.Message);
                return NotFound();
            }
        }

        [HttpGet]
        public async Task<IActionResult> LoginAdmin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginAdmin(Contact contact)
        {
            var res = _serviceContact.GetById(contact.id);
            if (res.id == contact.id && res.FirstName == contact.FirstName)
            {
                await Authenticate(res);
                return RedirectToAction("TestForAdmin", "Main");
            }
            else
            {
                ModelState.AddModelError("", "ошибочка");
                return View();
            }
        }


        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetRegistrationView(string returnUrl)
        {
            return View();
        }

        private async Task Authenticate(DomainObj model)
        {
            if (model.Equals(null))
                throw new ArgumentNullException();
            var contact = new Contact();
            var manager = new Manager();
            var claims = new List<Claim>();
            if (model.GetType() == contact.GetType())
            {

                contact = (Contact)model;
                if (contact.FirstName == "Admin")
                {
                    claims.Add(new Claim(ClaimTypes.Email, model.Email));
                    claims.Add(new Claim(ClaimTypes.Role, "Administrator"));
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Email, model.Email));
                    claims.Add(new Claim(ClaimTypes.Role, "Contact"));
                }
              
            }
            if (model.GetType() == manager.GetType())
            {
                manager = (Manager)model;
                claims.Add(new Claim(ClaimTypes.Email, model.Email));
                claims.Add(new Claim(ClaimTypes.Role, "Manager"));
            }
            ClaimsIdentity identityData = new ClaimsIdentity(claims, "ApplicationCookie");
            await HttpContext.SignInAsync("Cookie", new ClaimsPrincipal(identityData));
            _logger.LogDebug("{model} success sign in, add cookies {roles}", model.Email, identityData.RoleClaimType);
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync("Cookie");
            return RedirectToAction("GetRegistrationView");
        }
    }
}



