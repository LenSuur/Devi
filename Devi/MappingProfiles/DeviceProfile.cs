using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Devi.Data;
using Devi.Models;

namespace Devi.MappingProfiles;
[ExcludeFromCodeCoverage]
public class DeviceProfile: Profile
{
    public DeviceProfile()
    {
        CreateMap<Device, DeviceModel>();
        CreateMap<DeviceModel, Device>();
        CreateMap<Device, Device>();
    }
}