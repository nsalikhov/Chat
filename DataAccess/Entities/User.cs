namespace DataAccess.Entities
{
	public class User
	{
		public long Id { get; set; }

		public string Login { get; set; }

		public string PasswordHash { get; set; }

		public string PasswordSalt { get; set; }
	}
}
