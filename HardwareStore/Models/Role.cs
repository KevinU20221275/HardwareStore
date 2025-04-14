using System.ComponentModel.DataAnnotations;

namespace HardwareStore.Models
{
    public class Role
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(50)]
        [Display(Name = "Nombre")]
        public string? Name { get; set; }
    }
}
