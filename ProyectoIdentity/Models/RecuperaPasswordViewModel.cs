using System.ComponentModel.DataAnnotations;

namespace ProyectoIdentity.Models
{
	public class RecuperaPasswordViewModel
	{
		[Required(ErrorMessage = "El Email es Obligatorio")]
		[EmailAddress]
		public string Email { get; set; }

		[Required(ErrorMessage = "El Password es Obligatorio")]
		[DataType(DataType.Password)]
		[Display(Name = "Contraseña")]
		public string Password { get; set; }

		[Required(ErrorMessage = "La confirmacion del Password es Obligatorio")]
		[Compare("Password", ErrorMessage = "La contraseña y confirmacion de contraseña no coiciden")]
		[DataType(DataType.Password)]
		[Display(Name = "Confirmar Contraseña")]
		public string ConfirmPassword { get; set; }

		public string Code { get; set; }

	}
}
