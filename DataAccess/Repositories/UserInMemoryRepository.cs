using System.Collections.Concurrent;
using System.Threading.Tasks;

using DataAccess.Entities;
using DataAccess.Exceptions;



namespace DataAccess.Repositories
{
	public class UserInMemoryRepository : IUserRepository
	{
		#region Implementation of IUserRepository

		public Task Add(User user)
		{
			if (!Users.TryAdd(user.Login, user))
			{
				throw new EntityAlreadyExistsException();
			}

			return Task.FromResult(false);
		}

		public Task<User> GetByLogin(string login)
		{
			User user;
			if (!Users.TryGetValue(login, out user))
			{
				throw new EntityNotFoundException();
			}

			return Task.FromResult(user);
		}

		#endregion

		private static readonly ConcurrentDictionary<string, User> Users = new ConcurrentDictionary<string, User>();
	}
}
