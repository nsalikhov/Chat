using System;
using System.Linq;
using System.Net.WebSockets;
using System.Security.Principal;
using System.Threading.Tasks;

using Chat.Chat;
using Chat.DataContracts;



namespace Chat.Processors
{
	public class ChatProcessor : IChatProcessor
	{
		public ChatProcessor(IChatService chatService, IChatEventSender chatEventSender)
		{
			_chatService = chatService;
			_chatEventSender = chatEventSender;
		}

		#region Implementation of IChatProcessor

		public void AddUser(IIdentity user, WebSocket webSocket)
		{
			_chatService.AddUser(user.Name, webSocket);

			_chatEventSender.SendPublic(
				new ChatEvent<string[]>
				{
					Type = ChatEventType.UsersList,
					Data = _chatService.Users.Select(x => x.Key).ToArray()
				});
		}

		public void RemoveUser(IIdentity user)
		{
			_chatService.RemoveUser(user.Name);

			_chatEventSender.SendPublic(
				new ChatEvent<string[]>
				{
					Type = ChatEventType.UsersList,
					Data = _chatService.Users.Select(x => x.Key).ToArray()
				});
		}

		public Task ProcessMessage(WebSocketMessageType messageType, byte[] requestData)
		{
			throw new NotImplementedException();
		}

		#endregion

		private readonly IChatEventSender _chatEventSender;
		private readonly IChatService _chatService;
	}
}
