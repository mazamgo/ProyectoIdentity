using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProyectoIdentity.Models;

namespace ProyectoIdentity.Controllers
{
    public class CuentasController : Controller
    {
        //Inyeccion de dependencias
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _singInManager;

        public CuentasController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> singInManager)
        {
            _userManager = userManager;
            _singInManager = singInManager;
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
              
                var resultado = await _singInManager.PasswordSignInAsync(accViewModel.Email,accViewModel.Password,accViewModel.RememberMe,lockoutOnFailure: false);

                if (resultado.Succeeded)
                {
					//return RedirectToAction("Index", "Home");
					//return Redirect(returnurl);
					return LocalRedirect(returnurl);
				}
				else
                {
                    ModelState.AddModelError(String.Empty, "Accesp Invalido");
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

    }
}
