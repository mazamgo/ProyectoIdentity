using System.ComponentModel.DataAnnotations;

namespace ProyectoIdentity.Models
{
    public class RegistroViewModel
    {
        [Required(ErrorMessage = "El Email es Obligatorio")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "El Password es Obligatorio")]
        [StringLength(50, ErrorMessage = "El {0} dee estar entre al menos {2}, caracters de longitud",MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Required(ErrorMessage = "La confirmacion del Password es Obligatorio")]
        [Compare("Password", ErrorMessage = "La contraseña y confirmacion de contraseña no coiciden")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Contraseña")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "El Mombre es Obligatorio")]
        public string Nombre { get; set; }

        public string Url { get; set; }

        public Int32 CodigoPais { get; set; }

        public string Telefono { get; set; }
        [Required(ErrorMessage = "El Pais es Obligatorio")]
        public string Pais { get; set; }
        public string Ciudad { get; set; }
        public string Direccion { get; set; }
        [Required(ErrorMessage = "La Fecha de Nacimiento es Obligatorio")]
        [Display(Name ="Fecha de Nacimiento")]
        public DateTime FechaNacimiento { get; set; }
        [Required(ErrorMessage = "El Estado es Obligatorio")]
        public bool Estado { get; set; }


    }
}
