using System.ComponentModel.DataAnnotations;

namespace HardwareStore.Models
{
    public class UpdatePassword
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Por favor confirme la contraseña")]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        public string? RePassword { get; set; }
    }
}
