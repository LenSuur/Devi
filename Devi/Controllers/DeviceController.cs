using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Devi.Data;
using Devi.Models;
using Devi.Services;

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
        public async Task<IActionResult> Details(int id)
        {
            var result = await _deviceService.GetDeviceById(id);

            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Device device)
        {
            await _deviceService.AddDevice(device);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _deviceService.GetDeviceById(id);

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Device device)
        {
            await _deviceService.UpdateDevice(device);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _deviceService.GetDeviceById(id);

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
