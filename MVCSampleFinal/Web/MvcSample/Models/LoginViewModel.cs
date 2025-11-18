using System.ComponentModel.DataAnnotations;

namespace MvcSample.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El usuario es requerido")]
        [Display(Name = "Usuario")]
        public string Usuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es requerida")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Contraseña { get; set; } = string.Empty;

        [Display(Name = "Recordar mi Contraseña")]
        public bool RecordarContraseña { get; set; }
    }
}

