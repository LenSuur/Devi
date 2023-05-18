using System.Diagnostics.CodeAnalysis;

namespace Devi.Models;
[ExcludeFromCodeCoverage]
public class DeviceModel
{
    public int Id { get; set; }
    public string DeviceName { get; set; }
    public string SerialNumber { get; set; }
    public decimal Price { get; set; }
    public string? Receipt { get; set; }
    public DateTime BoughtOn { get; set; }
    public DateTime WarrantyTill { get; set; }
}