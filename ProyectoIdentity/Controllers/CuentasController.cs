using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using ProyectoIdentity.Models;

namespace ProyectoIdentity.Controllers
{
    public class CuentasController : Controller
    {
        //Inyeccion de dependencias
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _singInManager;
        private readonly IEmailSender _emailSender;

        public CuentasController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> singInManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _singInManager = singInManager;
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {			
			return View();
        }

        [HttpGet]
        public async Task<IActionResult> Registro(string returnurl = null)
        {
			ViewData["ReturnUrl"] = returnurl;
			RegistroViewModel registroVM = new RegistroViewModel();

            return View(registroVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(RegistroViewModel rgViewModel,string returnurl = null)
		{
			ViewData["ReturnUrl"] = returnurl;
			returnurl = returnurl ?? Url.Content("~/");

			if (ModelState.IsValid) 
            { 
                var usuario = new AppUsuario 
                { 
                    UserName = rgViewModel.Email, 
                    Email = rgViewModel.Email,
                    Nombre = rgViewModel.Nombre,
                    Url = rgViewModel.Url,
                    CodigoPais = rgViewModel.CodigoPais,
                    Telefono = rgViewModel.Telefono,
                    Pais = rgViewModel.Pais,
                    Ciudad = rgViewModel.Ciudad,
                    Direccion = rgViewModel.Direccion,
                    FechaNacimiento = rgViewModel.FechaNacimiento,
                    Estado = rgViewModel.Estado

                };

                var resultado = await _userManager.CreateAsync(usuario,rgViewModel.Password);

                if(resultado.Succeeded)
                {
                    //Implementacion de confirmacion de email en el registro 24/24/2024
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(usuario);
					var urlRetorno = Url.Action("ConfirmarEmail", "Cuentas", new { userId = usuario.Id, code = code }, protocol: HttpContext.Request.Scheme);
					await _emailSender.SendEmailAsync(rgViewModel.Email, "Confirmar su Cuenta - Proyecto Identity",
				  "Por favor confirme su cuenta dando clic aqui: <a href=\"" + urlRetorno + "\">enlace</a>");

					//Para autenticarlo.
					await _singInManager.SignInAsync(usuario, isPersistent: false);

					//return RedirectToAction("Index", "Home");
					return LocalRedirect(returnurl);
				}

				ValidarErrres(resultado);

			}

            return View(rgViewModel);

		}


        private void ValidarErrres(IdentityResult resultado)
        {
            foreach(var error in resultado.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        //Metodo mostrar formulario de acceso
        [HttpGet]
        public IActionResult Acceso(string returnurl = null)
        {
            ViewData["ReturnUrl"] = returnurl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Acceso(AccesoViewModel accViewModel, string returnurl = null)
        {
			ViewData["ReturnUrl"] = returnurl;
            returnurl = returnurl ?? Url.Content("~/");

			if (ModelState.IsValid)
            {
              
                var resultado = await _singInManager.PasswordSignInAsync(accViewModel.Email,accViewModel.Password,accViewModel.RememberMe,lockoutOnFailure: true);

                if (resultado.IsLockedOut)
                {					
					return View("Bloqueado");
				}
				if (resultado.Succeeded)
				{
					//return RedirectToAction("Index", "Home");
					//return Redirect(returnurl);
					return LocalRedirect(returnurl);
				}
				else
                {
                    ModelState.AddModelError(String.Empty, "Acceso Invalido");
                    return View(accViewModel);
                }                

            }

            return View(accViewModel);

        }

        //Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SalirAplicacion()
        {
            await _singInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        //Olvido de Contraseña.
        [HttpGet]
        public IActionResult OlvidoPassword()
        {
            return View();
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> OlvidoPassword(OlvidoPasswordViewModel opViewModel)
		{
            if (ModelState.IsValid)
            {
                var usuario = await _userManager.FindByEmailAsync(opViewModel.Email); 

                if(usuario == null)
                {
                    return RedirectToAction("ConfirmacionOlvidoPassword");
                }

                var codigo = await _userManager.GeneratePasswordResetTokenAsync(usuario);
                var urlRetorno = Url.Action("ResetPassword", "Cuentas", new { userId = usuario.Id, code = codigo },protocol: HttpContext.Request.Scheme);
               
                await _emailSender.SendEmailAsync(opViewModel.Email, "Recuperar contraseña - Proyecto Identity",
                    "Por favor recupere su contraseña dando clic aqui: <a href=\"" + urlRetorno + "\">enlace</a>");

                return RedirectToAction("ConfirmacionOlvidoPassword");
            }	

            return View(opViewModel);
		}

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ConfirmacionOlvidoPassword()
        {
            return View();
        }

		[HttpGet]		
		public IActionResult ResetPassword(string code = null)
		{
			return code == null ? View("Error") : View();
		}

        [HttpPost]
        [ValidateAntiForgeryToken]		
		public async Task<IActionResult> ResetPassword(RecuperaPasswordViewModel rpViewModel)
        {
            if (ModelState.IsValid)
            {
                var usuario = await _userManager.FindByEmailAsync(rpViewModel.Email);

                if (usuario == null)
                {
                    return RedirectToAction("ConfirmacionRecuperaPassword");
                }

                var resultado = await _userManager.ResetPasswordAsync(usuario, rpViewModel.Code, rpViewModel.Password);
                if(resultado.Succeeded)
                {
                    return RedirectToAction("ConfirmacionRecuperaPassword");
                }

                ValidarErrres(resultado);
            }

            return View(rpViewModel);
        }

		[HttpGet]
        [AllowAnonymous]
		public IActionResult ConfirmacionRecuperaPassword()
		{
			return View();
		}

		//Metodos para la confirmacion de Email en el registro.
		[HttpGet]
		public async Task<IActionResult> ConfirmarEmail(string userId, string code)
		{
            if(userId == null || code == null)
            {
                return View("Error");
            }

            var usuario = await _userManager.FindByIdAsync(userId);
            if (usuario == null)
            {
				return View("Error");
			}

            var resultado = await _userManager.ConfirmEmailAsync(usuario, code);
			return View(resultado.Succeeded ? "ConfirmarEmail" : "Error");
		}
	}
}
