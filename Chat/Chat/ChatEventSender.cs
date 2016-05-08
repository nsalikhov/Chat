using System;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

using Chat.DataContracts;
using Chat.Serializers;



namespace Chat.Chat
{
	public class ChatEventSender : IChatEventSender
	{
		public ChatEventSender(IChatService chatService, IJsonSerializer jsonSerializer)
		{
			_chatService = chatService;
			_jsonSerializer = jsonSerializer;
		}

		#region Implementation of IChatEventSender

		public async Task SendPublic<T>(ChatEvent<T> chatEvent)
		{
			var data = new ArraySegment<byte>(_jsonSerializer.Serialize(chatEvent));

			var sendTasks = _chatService
				.Users
				.Select(x => x.Value.SendAsync(data, WebSocketMessageType.Text, true, CancellationToken.None));

			await Task.WhenAll(sendTasks);
		}

		public Task SendPrivate<T>(string recipient, ChatEvent<T> chatEvent)
		{
			throw new NotImplementedException();
		}

		#endregion

		private readonly IChatService _chatService;

		private readonly IJsonSerializer _jsonSerializer;
	}
}
