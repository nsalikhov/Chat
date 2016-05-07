using System.Web.Mvc;

using Chat.Models;



namespace Chat.Controllers
{
	public class AuthenticationController : Controller
	{
		public ActionResult SignIn()
		{
			return View();
		}

		[HttpPost]
		public ActionResult SignIn(SignInModel model)
		{
			if (!ModelState.IsValid)
			{
				return View("SignIn");
			}

			return RedirectToAction("Index", "Home");
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
	}
}
