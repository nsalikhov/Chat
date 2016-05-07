namespace Chat.Security
{
	public interface IPasswordConverter
	{
		string GetPasswordHash(string password, string salt);
	}
}
