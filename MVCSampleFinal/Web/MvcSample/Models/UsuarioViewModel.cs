using System.ComponentModel.DataAnnotations;
using System;

namespace MvcSample.Models
{
    public class UsuarioViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [Display(Name = "Nombre Usuario")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo es requerido")]
        [EmailAddress(ErrorMessage = "El correo no es válido")]
        [Display(Name = "Correo")]
        public string Correo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es requerida")]
        [Display(Name = "Contraseña")]
        public string Contraseña { get; set; } = string.Empty;

        [Required(ErrorMessage = "El rol es requerido")]
        [Display(Name = "Rol")]
        public string Rol { get; set; } = string.Empty;
    }
}

