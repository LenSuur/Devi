using Devi.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Devi.Models
{
    public class Device: Entity
    {        
        [Required]
        [StringLength(50)]
        public string? DeviceName { get; set; }
        [Required]
        [StringLength(30)]
        public string? SerialNumber { get; set; }
        [Column(TypeName = "decimal(7,2)")]
        public decimal Price { get; set; }
        public string? Receipt { get; set; }
        [DataType(DataType.Date)]
        public DateTime BoughtOn { get; set; }
        [DataType(DataType.Date)]
        public DateTime WarrantyTill { get; set; }
    }
}