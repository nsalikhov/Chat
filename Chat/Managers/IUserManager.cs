using System.Threading.Tasks;

using DataAccess.Entities;



namespace Chat.Managers
{
	public interface IUserManager
	{
		Task<User> CreateUser(string login, string password);

		Task<bool> CheckUserPassword(string login, string password);
	}
}
