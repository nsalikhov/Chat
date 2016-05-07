using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

using Chat.Controllers;
using Chat.Managers;
using Chat.Models;
using Chat.Resources;
using Chat.Security;

using DataAccess.Entities;
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
			_authenticationService = new Mock<IAuthenticationService>(MockBehavior.Strict);

			_target = new AuthenticationController(_authenticationService.Object, _userManager.Object);
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

			_authenticationService.Setup(x => x.Authenticate(model.Login, model.Password, model.RememberMe)).Returns(Task.FromResult(true));

			var result = _target.SignIn(model).Result as RedirectToRouteResult;

			_authenticationService.Verify(x => x.Authenticate(model.Login, model.Password, model.RememberMe), Times.Once);

			Assert.IsNotNull(result);
			Assert.AreEqual("Index", result.RouteValues["action"]);
			Assert.AreEqual("Home", result.RouteValues["controller"]);
		}

		[TestMethod]
		public void SignIn_Post_InvalidModel_Test()
		{
			var model = _fixture.Create<SignInModel>();

			_target.ModelState.AddModelError(_fixture.Create<string>(), _fixture.Create<string>());

			var result = _target.SignIn(model).Result as ViewResult;

			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void SignIn_Post_InvalidPassword_Test()
		{
			var model = _fixture.Create<SignInModel>();

			_authenticationService.Setup(x => x.Authenticate(model.Login, model.Password, model.RememberMe)).Returns(Task.FromResult(false));

			var result = _target.SignIn(model).Result as ViewResult;

			_authenticationService.Verify(x => x.Authenticate(model.Login, model.Password, model.RememberMe), Times.Once);

			Assert.IsNotNull(result);
			Assert.AreEqual(AuthenticationResource.InvalidPasswordErrorMessage, _target.ModelState["Login"].Errors.Single().ErrorMessage);
		}

		[TestMethod]
		public void SignIn_Post_UserNotFound_Test()
		{
			var model = _fixture.Create<SignInModel>();

			_authenticationService.Setup(x => x.Authenticate(model.Login, model.Password, model.RememberMe)).ThrowsAsync(new EntityNotFoundException());

			var result = _target.SignIn(model).Result as ViewResult;

			_authenticationService.Verify(x => x.Authenticate(model.Login, model.Password, model.RememberMe), Times.Once);

			Assert.IsNotNull(result);
			Assert.AreEqual(
				string.Format(AuthenticationResource.UserNotFoundErrorMessage, model.Login),
				_target.ModelState["Login"].Errors.Single().ErrorMessage);
		}

		[TestMethod]
		public void SignUp_Get_Test()
		{
			var result = _target.SignUp();

			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void SignUp_Post_Test()
		{
			var model = _fixture.Create<SignUpModel>();

			_userManager.Setup(x => x.CreateUser(model.Login, model.Password)).Returns(Task.FromResult<User>(null));

			var result = _target.SignUp(model).Result as RedirectToRouteResult;

			_userManager.Verify(x => x.CreateUser(model.Login, model.Password), Times.Once);

			Assert.IsNotNull(result);
			Assert.AreEqual("SignIn", result.RouteValues["action"]);
		}

		[TestMethod]
		public void SignUp_Post_InvalidModel_Test()
		{
			var model = _fixture.Create<SignUpModel>();

			_target.ModelState.AddModelError(_fixture.Create<string>(), _fixture.Create<string>());

			var result = _target.SignUp(model).Result as ViewResult;

			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void SignUp_Post_UserAlreadyExists_Test()
		{
			var model = _fixture.Create<SignUpModel>();

			_userManager.Setup(x => x.CreateUser(model.Login, model.Password)).ThrowsAsync(new EntityAlreadyExistsException());

			var result = _target.SignUp(model).Result as ViewResult;

			_userManager.Verify(x => x.CreateUser(model.Login, model.Password), Times.Once);

			Assert.IsNotNull(result);
			Assert.AreEqual(
				string.Format(AuthenticationResource.UserAlreadyExistsErrorMessage, model.Login),
				_target.ModelState["Login"].Errors.Single().ErrorMessage);
		}

		[TestMethod]
		public void SignOut_Test()
		{
			_authenticationService.Setup(x => x.SignOut());

			var result = _target.SignOut();

			_authenticationService.Verify(x => x.SignOut(), Times.Once);

			Assert.IsNotNull(result);
			Assert.AreEqual("Index", result.RouteValues["action"]);
			Assert.AreEqual("Home", result.RouteValues["controller"]);
		}

		private IFixture _fixture;
		private AuthenticationController _target;
		private Mock<IUserManager> _userManager;
		private Mock<IAuthenticationService> _authenticationService;
	}
}
