using System.Threading.Tasks;



namespace Chat.Security
{
	public interface IAuthenticationService
	{
		Task<bool> Authenticate(string login, string password, bool persistentSession);

		void SignOut();
	}
}
