using System.ComponentModel.DataAnnotations;

namespace ExternalService.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "El campo Usuario es obligatorio.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El Usuario debe tener entre 3 y 50 caracteres.")]
        public string User { get; set; }

        [Required(ErrorMessage = "El campo Contraseña es obligatorio.")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "La Contraseña debe tener entre 6 y 20 caracteres.")]
        public string Password { get; set; }
    }
}
