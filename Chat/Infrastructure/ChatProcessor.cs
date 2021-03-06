﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Security.Principal;
using System.Threading.Tasks;

using Chat.DataContracts;
using Chat.Infrastructure.MessageProcessors;



namespace Chat.Infrastructure
{
	public class ChatProcessor : IChatProcessor
	{
		public ChatProcessor(
			IChatEventSender chatEventSender,
			IReadOnlyDictionary<WebSocketMessageType, IChatMessageProcessor> chatMessageProcessors,
			IChatService chatService)
		{
			_chatEventSender = chatEventSender;
			_chatMessageProcessors = chatMessageProcessors;
			_chatService = chatService;
		}

		#region Implementation of IChatProcessor

		public void AddUser(IIdentity user, WebSocket webSocket)
		{
			_chatService.AddUser(user.Name, webSocket);

			_chatEventSender.SendPublic(
				new ChatEvent<ChatUsersList>
				{
					Type = ChatEventType.UsersList,
					Data = new ChatUsersList
					{
						UpdateMessage = $"{user.Name} has joined the chat.",
						Users = _chatService.Users.Select(x => x.Key).ToArray()
					}
				});
		}

		public void RemoveUser(IIdentity user)
		{
			_chatService.RemoveUser(user.Name);

			_chatEventSender.SendPublic(
				new ChatEvent<ChatUsersList>
				{
					Type = ChatEventType.UsersList,
					Data = new ChatUsersList
					{
						UpdateMessage = $"{user.Name} has left the chat.",
						Users = _chatService.Users.Select(x => x.Key).ToArray()
					}
				});
		}

		public async Task ProcessMessage(IIdentity user, WebSocket webSocket, WebSocketMessageType messageType, byte[] requestData)
		{
			IChatMessageProcessor msgProcessor;
			if (_chatMessageProcessors.TryGetValue(messageType, out msgProcessor))
			{
				await msgProcessor.Process(user.Name, webSocket, requestData);

				return;
			}

			throw new InvalidOperationException("Unknown web socket message type.");
		}

		#endregion

		private readonly IChatEventSender _chatEventSender;
		private readonly IReadOnlyDictionary<WebSocketMessageType, IChatMessageProcessor> _chatMessageProcessors;
		private readonly IChatService _chatService;
	}
}
