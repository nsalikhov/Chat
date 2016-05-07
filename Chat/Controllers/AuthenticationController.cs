using System.Threading.Tasks;
using System.Web.Mvc;

using Chat.Helpers;
using Chat.Managers;
using Chat.Models;
using Chat.Resources;

using DataAccess.Exceptions;



namespace Chat.Controllers
{
	public class AuthenticationController : Controller
	{
		public AuthenticationController(IUserManager userManager)
		{
			_userManager = userManager;
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
					if (await _userManager.CheckUserPassword(model.Login, model.Password))
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

			return View("SignUp");
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

		private readonly IUserManager _userManager;
	}
}
