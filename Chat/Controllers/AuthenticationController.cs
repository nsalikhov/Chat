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
		public ActionResult SignIn(UserModel model)
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
		public ActionResult SignUp(UserModel model)
		{
			if (!ModelState.IsValid)
			{
				return View("SignIn");
			}

			return RedirectToAction("Index", "Home");
		}
	}
}
