using System.Linq;
using System.Web.Mvc;

using Chat.Controllers;
using Chat.Helpers;
using Chat.Managers;
using Chat.Processors;
using Chat.Security;
using Chat.Serializers;
using Chat.Services;

using DataAccess.Repositories;

using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Mvc;



namespace Chat
{
	public static class UnityConfig
	{
		public static void Initialize()
		{
			var container = new UnityContainer();

			RegisterTypes(container);

			FilterProviders.Providers.Remove(FilterProviders.Providers.OfType<FilterAttributeFilterProvider>().First());
			FilterProviders.Providers.Add(new UnityFilterAttributeFilterProvider(container));

			DependencyResolver.SetResolver(new UnityDependencyResolver(container));
		}

		private static void RegisterTypes(IUnityContainer container)
		{
			container.RegisterType<IGuidProvider, GuidProvider>(new ContainerControlledLifetimeManager());
			container.RegisterType<IPasswordConverter, PasswordConverter>(new ContainerControlledLifetimeManager());
			container.RegisterType<IJsonSerializer, NewtonsoftJsonSerializer>();

			container.RegisterType<IAuthenticationService, AuthenticationService>();
			container.RegisterType<IUserRepository, UserInMemoryRepository>();
			container.RegisterType<IUserManager, UserManager>();
			container.RegisterType<IChatProcessor, ChatProcessor>();
			container.RegisterType<IChatService, ChatService>();

			container.RegisterType<AuthenticationController>();
			container.RegisterType<HomeController>();
			container.RegisterType<ChatController>(
				new InjectionFactory(
					c => new ChatController(
						Settings.MessageBufferSizeBytes,
						Settings.MaxMessageSizeBytes,
						c.Resolve<IChatProcessor>())));
		}
	}
}
