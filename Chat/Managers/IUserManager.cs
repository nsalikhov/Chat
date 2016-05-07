using DataAccess.Entities;



namespace Chat.Managers
{
	public interface IUserManager
	{
		User CreateUser(string login, string password);

		bool CheckUserPassword(string login, string password);
	}
}
