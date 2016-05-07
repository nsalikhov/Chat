using DataAccess.Entities;



namespace DataAccess.Repositories
{
	public interface IUserRepository
	{
		void Add(User user);

		User GetByLogin(string login);
	}
}
