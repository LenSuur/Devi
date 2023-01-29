using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Devi.Data.Repositories;
using Devi.Models;
using Microsoft.AspNetCore.Mvc;

public interface IDeviceService
{
    Task<List<Device>> GetAllDevices();
    Task<Device> GetDeviceById(int id);
    Task AddDevice(Device device);
    Task UpdateDevice(Device device);
    Task DeleteDevice(int id);
}


