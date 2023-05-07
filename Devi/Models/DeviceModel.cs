namespace Devi.Models;

public class DeviceModel
{
    public int Id { get; set; }
    public string DeviceName { get; set; }
    public string SerialNumber { get; set; }
    public double Price { get; set; }
    public string? Receipt { get; set; }
    public DateTime BoughtOn { get; set; }
    public DateTime WarrantyTill { get; set; }
}