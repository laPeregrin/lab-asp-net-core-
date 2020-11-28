using BL_BusinessLogic_;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using ObjectContainer.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebView.Loggers;

namespace WebView.Controllers
{
    public class ManagerController : Controller
    {
        private readonly ILogger<ManagerController> _logger;
        private readonly IMemoryCache _cache;
        private readonly ManagerBLL _service;

        public ManagerController(ILogger<ManagerController> logger, ManagerBLL service, IMemoryCache cache)
        {
            _logger = logger;
            _service = service;
            _cache = cache;
        }

        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<Manager> managers;
                if (!_cache.TryGetValue("managers", out managers))
                {
                    managers = (List<Manager>)_service.GetAll().Result;
                    await Task.Run(() =>
                    {
                        _cache.Set("managers", managers, new MemoryCacheEntryOptions()
                        { SlidingExpiration = TimeSpan.FromSeconds(1000) }
                        );
                        _logger.LogDebug("______Cached Managers in other thread {thread}", Thread.CurrentThread.ThreadState);
                    });
                    return View(managers);
                }
                else
                {
                    _logger.LogDebug("Take cached data");
                    return View(managers);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "___Error {message}", e.Message);
                return NotFound();
            }

        }


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
                    var manager = _service.GetById(id.Value).Result;
                    return View(manager);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Take error{e}", e.Message);
                return RedirectToAction("GetAll", "Manager");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id != null)
            {
                await _service.Delete(id);
                var task = Task.Run(() => _cache.Remove("managers"));
                return RedirectToAction("GetAll", "Manager");
            }
            return NotFound();
        }
    }
}
