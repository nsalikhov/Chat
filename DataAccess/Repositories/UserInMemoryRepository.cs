using System.Collections.Concurrent;

using DataAccess.Entities;
using DataAccess.Exceptions;



namespace DataAccess.Repositories
{
	public class UserInMemoryRepository : IUserRepository
	{
		#region Implementation of IUserRepository

		public void Add(User user)
		{
			if (!Users.TryAdd(user.Login, user))
			{
				throw new EntityAlreadyExistsException();
			}
		}

		public User GetByLogin(string login)
		{
			User user;
			if (!Users.TryGetValue(login, out user))
			{
				throw new EntityNotFoundException();
			}

			return user;
		}

		#endregion

		private static readonly ConcurrentDictionary<string, User> Users = new ConcurrentDictionary<string, User>();
	}
}
