using AutoMapper;
using Devi.Data;
using Devi.Models;

namespace Devi.MappingProfiles;

public class DeviceProfile: Profile
{
    public DeviceProfile()
    {
        CreateMap<Device, DeviceModel>();
        CreateMap<DeviceModel, Device>();
        CreateMap<Device, Device>();
    }
}