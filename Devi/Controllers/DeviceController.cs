
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Devi.Data;
using Devi.Models;
using Devi.Services;

namespace Devi.Controllers
{
    public class DeviceController : Controller
    {
        private readonly IDeviceService _deviceService;
        private readonly IFileClient _fileClient;
        public DeviceController(IDeviceService deviceService, IFileClient fileClient)
        {
            _deviceService = deviceService;
            _fileClient = fileClient;
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
        public async Task<IActionResult> Create(DeviceModel model, IFormFile? file)
        {
            if (file == null || file.Length == 0)
            {
                return Content("file not selected");
            }

            await using (var stream = file.OpenReadStream())
            {
                model.Receipt = file.FileName;
                await _fileClient.Save(FileContainerNames.Receipts, file.FileName, stream);
            }
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

        [HttpPost]
        public async Task<IActionResult> SingleFile([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return Content("file not selected");
            }

            await using (var stream = file.OpenReadStream())
            {
                await _fileClient.Save(FileContainerNames.Receipts, file.FileName, stream);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Download(DeviceModel model)
        {
            var fileStream = await _fileClient.Get(FileContainerNames.Receipts, model.Receipt);

            if (fileStream == null)
            {
                return Content("File not found.");
            }

            var contentDisposition = new ContentDisposition
            {
                FileName = model.Receipt,
                Inline = false
            };
    
            Response.Headers.Add("Content-Disposition", contentDisposition.ToString());
            Response.Headers.Add("Content-Length", fileStream.Length.ToString());

            return File(fileStream, "application/octet-stream");
        }
    }
}
