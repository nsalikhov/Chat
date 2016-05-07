using System.Threading.Tasks;
using System.Web.Security;

using Chat.Managers;



namespace Chat.Security
{
	public class AuthenticationService : IAuthenticationService
	{
		public AuthenticationService(IUserManager userManager)
		{
			_userManager = userManager;
		}

		#region Implementation of IAuthenticationService

		public async Task<bool> Authenticate(string login, string password, bool persistentSession)
		{
			var authResult = await _userManager.CheckUserPassword(login, password);
			if (authResult)
			{
				FormsAuthentication.SetAuthCookie(login, persistentSession);
			}

			return authResult;
		}

		public void SignOut()
		{
			FormsAuthentication.SignOut();
		}

		#endregion

		private readonly IUserManager _userManager;
	}
}
