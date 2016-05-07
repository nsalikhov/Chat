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
		public ActionResult SignIn(SignInModel model)
		{
			if (ModelState.IsValid)
			{
				try
				{
					if (_userManager.CheckUserPassword(model.Login, model.Password))
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
		public ActionResult SignUp(SignUpModel model)
		{
			if (!ModelState.IsValid)
			{
				return View("SignUp");
			}

			return RedirectToAction("Index", "Home");
		}

		private readonly IUserManager _userManager;
	}
}
