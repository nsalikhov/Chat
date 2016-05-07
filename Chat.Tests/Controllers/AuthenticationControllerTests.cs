﻿using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web.Mvc;

using Chat.Controllers;
using Chat.Managers;
using Chat.Models;
using Chat.Resources;

using DataAccess.Exceptions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using Ploeh.AutoFixture;



namespace Chat.Tests.Controllers
{
	[ExcludeFromCodeCoverage]
	[TestClass]
	public class AuthenticationControllerTests
	{
		[TestInitialize]
		public void TestInitialize()
		{
			_fixture = new Fixture();
			_userManager = new Mock<IUserManager>(MockBehavior.Strict);

			_target = new AuthenticationController(_userManager.Object);
		}

		[TestMethod]
		public void SignIn_Get_Test()
		{
			var result = _target.SignIn();

			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void SignIn_Post_Test()
		{
			var model = _fixture.Create<SignInModel>();

			_userManager.Setup(x => x.CheckUserPassword(model.Login, model.Password)).Returns(true);

			var result = _target.SignIn(model) as RedirectToRouteResult;

			_userManager.Verify(x => x.CheckUserPassword(model.Login, model.Password), Times.Once);

			Assert.IsNotNull(result);
			Assert.AreEqual("Index", result.RouteValues["action"]);
			Assert.AreEqual("Home", result.RouteValues["controller"]);
		}

		[TestMethod]
		public void SignIn_Post_InvalidModel_Test()
		{
			var model = _fixture.Create<SignInModel>();

			_target.ModelState.AddModelError(_fixture.Create<string>(), _fixture.Create<string>());

			var result = _target.SignIn(model) as ViewResult;

			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void SignIn_Post_InvalidPassword_Test()
		{
			var model = _fixture.Create<SignInModel>();

			_userManager.Setup(x => x.CheckUserPassword(model.Login, model.Password)).Returns(false);

			var result = _target.SignIn(model) as ViewResult;

			_userManager.Verify(x => x.CheckUserPassword(model.Login, model.Password), Times.Once);

			Assert.IsNotNull(result);
			Assert.AreEqual(AuthenticationResource.InvalidPasswordErrorMessage, _target.ModelState["Login"].Errors.Single().ErrorMessage);
		}

		[TestMethod]
		public void SignIn_Post_UserNotFound_Test()
		{
			var model = _fixture.Create<SignInModel>();

			_userManager.Setup(x => x.CheckUserPassword(model.Login, model.Password)).Throws<EntityNotFoundException>();

			var result = _target.SignIn(model) as ViewResult;

			_userManager.Verify(x => x.CheckUserPassword(model.Login, model.Password), Times.Once);

			Assert.IsNotNull(result);
			Assert.AreEqual(string.Format(AuthenticationResource.UserNotFoundErrorMessage, model.Login), _target.ModelState["Login"].Errors.Single().ErrorMessage);
		}

		private IFixture _fixture;
		private Mock<IUserManager> _userManager;
		private AuthenticationController _target;
	}
}
