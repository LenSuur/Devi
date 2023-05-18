using Devi.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Devi.Data
{
    [ExcludeFromCodeCoverage]
    public class Device: Entity
    {        
        [Required]
        [StringLength(50)]
        public string DeviceName { get; set; }
        [Required]
        [StringLength(30)]
        public string SerialNumber { get; set; }
        public decimal Price { get; set; }
        public string? Receipt { get; set; }
        [DataType(DataType.Date)]
        public DateTime BoughtOn { get; set; }
        [DataType(DataType.Date)]
        public DateTime WarrantyTill { get; set; }
    }
}