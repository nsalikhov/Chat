using Chat.Helpers;
using Chat.Security;

using DataAccess.Entities;
using DataAccess.Repositories;



namespace Chat.Managers
{
	public class UserManager : IUserManager
	{
		public UserManager(IUserRepository userRepository, IPasswordConverter passwordConverter, IGuidProvider guidProvider)
		{
			_userRepository = userRepository;
			_passwordConverter = passwordConverter;
			_guidProvider = guidProvider;
		}

		#region Implementation of IUserManager

		public User CreateUser(string login, string password)
		{
			var salt = _guidProvider.NewGuid().ToString();
			var passwordHash = _passwordConverter.GetPasswordHash(password, salt);

			var user = new User
			{
				Login = login,
				PasswordHash = passwordHash,
				PasswordSalt = salt
			};

			_userRepository.Add(user);

			return user;
		}

		public bool CheckUserPassword(string login, string password)
		{
			var user = _userRepository.GetByLogin(login);

			var hash = _passwordConverter.GetPasswordHash(password, user.PasswordSalt);

			return user.PasswordHash == hash;
		}

		#endregion

		private readonly IGuidProvider _guidProvider;
		private readonly IPasswordConverter _passwordConverter;
		private readonly IUserRepository _userRepository;
	}
}
