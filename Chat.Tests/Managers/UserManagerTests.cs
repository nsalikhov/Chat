using System;
using System.Diagnostics.CodeAnalysis;

using Chat.Helpers;
using Chat.Managers;
using Chat.Security;

using DataAccess.Entities;
using DataAccess.Repositories;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using Ploeh.AutoFixture;



namespace Chat.Tests.Managers
{
	[ExcludeFromCodeCoverage]
	[TestClass]
	public class UserManagerTests
	{
		[TestInitialize]
		public void TestInitialize()
		{
			_fixture = new Fixture();

			_guidProvider = new Mock<IGuidProvider>(MockBehavior.Strict);
			_passwordConverter = new Mock<IPasswordConverter>(MockBehavior.Strict);
			_userRepository = new Mock<IUserRepository>(MockBehavior.Strict);

			_target = new UserManager(_userRepository.Object, _passwordConverter.Object, _guidProvider.Object);
		}

		[TestMethod]
		public void CreateUser_Test()
		{
			var login = _fixture.Create<string>();
			var password = _fixture.Create<string>();
			var guid = _fixture.Create<Guid>();
			var passwordHash = _fixture.Create<string>();

			var expectedUser = new User
			{
				Login = login,
				PasswordHash = passwordHash,
				PasswordSalt = guid.ToString()
			};

			_guidProvider.Setup(x => x.NewGuid()).Returns(guid);
			_passwordConverter.Setup(x => x.GetPasswordHash(password, guid.ToString())).Returns(passwordHash);
			_userRepository.Setup(x => x.Add(It.Is<User>(u => CheckUser(expectedUser, u))));

			var user = _target.CreateUser(login, password);

			_guidProvider.Verify(x => x.NewGuid(), Times.Once);
			_passwordConverter.Verify(x => x.GetPasswordHash(password, guid.ToString()), Times.Once);
			_userRepository.Verify(x => x.Add(It.Is<User>(u => CheckUser(expectedUser, u))), Times.Once);

			Assert.IsTrue(CheckUser(expectedUser, user));
		}

		[TestMethod]
		public void CheckUserPassword_Test()
		{
			var login = _fixture.Create<string>();
			var password = _fixture.Create<string>();
			var user = _fixture.Create<User>();

			_userRepository.Setup(x => x.GetByLogin(login)).Returns(user);
			_passwordConverter.Setup(x => x.GetPasswordHash(password, user.PasswordSalt)).Returns(user.PasswordHash);

			var result = _target.CheckUserPassword(login, password);

			_userRepository.Verify(x => x.GetByLogin(login), Times.Once);
			_passwordConverter.Verify(x => x.GetPasswordHash(password, user.PasswordSalt), Times.Once);

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void CheckUserPassword_InvalidPassword_Test()
		{
			var login = _fixture.Create<string>();
			var password = _fixture.Create<string>();
			var user = _fixture.Create<User>();

			_userRepository.Setup(x => x.GetByLogin(login)).Returns(user);
			_passwordConverter.Setup(x => x.GetPasswordHash(password, user.PasswordSalt)).Returns(_fixture.Create<string>());

			var result = _target.CheckUserPassword(login, password);

			_userRepository.Verify(x => x.GetByLogin(login), Times.Once);
			_passwordConverter.Verify(x => x.GetPasswordHash(password, user.PasswordSalt), Times.Once);

			Assert.IsFalse(result);
		}

		private static bool CheckUser(User expected, User actual)
		{
			actual.ShouldBeEquivalentTo(expected);

			return true;
		}

		private Mock<IGuidProvider> _guidProvider;
		private Mock<IPasswordConverter> _passwordConverter;
		private Mock<IUserRepository> _userRepository;
		private IFixture _fixture;
		private UserManager _target;
	}
}
