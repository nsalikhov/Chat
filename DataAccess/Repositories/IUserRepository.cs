using System.Threading.Tasks;

using DataAccess.Entities;



namespace DataAccess.Repositories
{
	public interface IUserRepository
	{
		Task Add(User user);

		Task<User> GetByLogin(string login);
	}
}
