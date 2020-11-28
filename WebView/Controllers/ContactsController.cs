using System;
using System.Linq.Expressions;
using BL_BusinessLogic_;
using ObjectContainer.Objects;
using Microsoft.AspNetCore.Mvc;
using WebView.ActionFilters;
using Microsoft.Extensions.Logging;
using WebView.Loggers;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebView.Controllers
{
    [LogActionFilter]
    public class ContactsController : Controller
    {
        private IMemoryCache _cache;
        private readonly ILogger<ContactsController> _logger;
        private readonly ContactBLL _service;

        public ContactsController(ContactBLL service, ILogger<ContactsController> logger, IMemoryCache cache)
        {
            _service = service;
            _logger = logger;
            _cache = cache;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var name = Environment.UserName;
            ViewData["Message"] = name + " - is your SystemName";
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpGet]
        public async Task<IActionResult> GetById(Guid? id)
        {
            _logger.LogInformation(LogEvents.GetItem, "Get id - {id}", id);
            try
            {
                if (id == null)
                {
                    _logger.LogDebug(LogEvents.UpdateItemNotFound, "id is null");
                    return NotFound();
                }
                var user = _service.GetById(id.ToString()); 
                _logger.LogInformation("User - {firstName} {lastName}", user.FirstName, user.LastName);
                return View(user);

            }
            catch (Exception e)
            {
                _logger.LogError(e, "Take error{e}", e.Message);
                return RedirectToAction("Contacts", "Details");
            }
        }

        [HttpGet]
        public IActionResult Details()
        {
            List<Contact> collection;

            Expression<Func<Contact, bool>> expression = x => x.Visible == true;
            if (!_cache.TryGetValue("contacts", out collection))
            {
                collection = (List<Contact>)_service.GetAllVisible(expression);
                _cache.Set("contacts", collection,
                    new MemoryCacheEntryOptions { SlidingExpiration = TimeSpan.FromSeconds(1000) });
                _logger.LogDebug("_____Caching ContactsCollection - {collection}", collection.Count);
            }
            _logger.LogInformation("____Take from cache is {_cacheCollection}", collection);

            return View(collection);
        }

        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            var res = _service.Delete(id);
            if (res)
            {
                var res1 = Task.Run(() =>
                {
                    _cache.Remove("contacts");
                    _logger.LogDebug("delete cache from contacts");
                });
                var res2 = Task.Run(() =>
                {
                    _logger.LogDebug("delete cache from contact");
                    _cache.Remove("contact");
                });

                Task.WaitAll(res1, res2);
                return RedirectToAction("Details", "Contacts");
            }
            else
            {
                return View(id);
            }
        }
    }
}
