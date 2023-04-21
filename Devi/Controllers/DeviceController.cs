
using Microsoft.AspNetCore.Mvc;
using Devi.Data;
using Devi.Models;

namespace Devi.Controllers
{
    public class DeviceController : Controller
    {
        private readonly IDeviceService _deviceService;
        public DeviceController(IDeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        [HttpGet]        
        public async Task<IActionResult> Index()
        {
            var result = await _deviceService.GetAllDevices();

            return View(result);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var device = await _deviceService.Get<DeviceModel>(id.Value);
            if (device == null)
            {
                return NotFound();
            }

            return View(device);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(DeviceModel model)
        {
            await _deviceService.AddDevice(model);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _deviceService.Get<DeviceModel>(id);

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(DeviceModel model)
        {
            await _deviceService.UpdateDevice(model);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _deviceService.Get<DeviceModel>(id);

            return View(result);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _deviceService.DeleteDevice(id);

            return RedirectToAction("Index");
        }
    }
}
