using System.ComponentModel.DataAnnotations;

namespace ProyectoIdentity.Models
{
    public class OlvidoPasswordViewModel
	{
        [Required(ErrorMessage = "El Email es Obligatorio")]
        [EmailAddress]
        public string Email { get; set; }

    }
}
