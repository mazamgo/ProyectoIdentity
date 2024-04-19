using System.ComponentModel.DataAnnotations;

namespace ProyectoIdentity.Models
{
    public class AccesoViewModel
    {
        [Required(ErrorMessage = "El Email es Obligatorio")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "El Password es Obligatorio")]        
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Display(Name = "Recordar Datos?")]
        public bool RememberMe { get; set; }


    }
}
