namespace Chat.Managers
{
	public interface IUserManager
	{
		bool CheckUserPassword(string login, string password);
	}
}
