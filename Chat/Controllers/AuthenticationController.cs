using System.Threading.Tasks;
using System.Web.Mvc;

using Chat.Helpers;
using Chat.Managers;
using Chat.Models;
using Chat.Resources;
using Chat.Security;

using DataAccess.Exceptions;



namespace Chat.Controllers
{
	public class AuthenticationController : Controller
	{
		public AuthenticationController(IAuthenticationService authenticationService, IUserManager userManager)
		{
			_userManager = userManager;
			_authenticationService = authenticationService;
		}

		public ViewResult SignIn()
		{
			return View();
		}

		[HttpPost]
		public async Task<ActionResult> SignIn(SignInModel model)
		{
			if (ModelState.IsValid)
			{
				try
				{
					if (await _authenticationService.Authenticate(model.Login, model.Password, model.RememberMe))
					{
						return RedirectToAction("Index", "Home");
					}

					ModelState.AddModelError(m => model.Login, AuthenticationResource.InvalidPasswordErrorMessage);
				}
				catch (EntityNotFoundException)
				{
					ModelState.AddModelError(m => model.Login, string.Format(AuthenticationResource.UserNotFoundErrorMessage, model.Login));
				}
			}

			return View("SignIn");
		}

		public ActionResult SignUp()
		{
			return View();
		}

		[HttpPost]
		public async Task<ActionResult> SignUp(SignUpModel model)
		{
			if (ModelState.IsValid)
			{
				try
				{
					await _userManager.CreateUser(model.Login, model.Password);

					return RedirectToAction("SignIn");
				}
				catch (EntityAlreadyExistsException)
				{
					ModelState.AddModelError(m => model.Login, string.Format(AuthenticationResource.UserAlreadyExistsErrorMessage, model.Login));
				}
			}

			return View("SignUp");
		}

		private readonly IAuthenticationService _authenticationService;
		private readonly IUserManager _userManager;
	}
}
