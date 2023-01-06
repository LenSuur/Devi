using System.ComponentModel.DataAnnotations;

namespace Devi.Data
{
    public abstract class Entity
    {
        [Key]
        public int Id { get; set; }
    }
}
