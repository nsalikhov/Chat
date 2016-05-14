using System.Net.WebSockets;
using System.Threading.Tasks;

using Chat.DataContracts;
using Chat.Serializers;



namespace Chat.Infrastructure.MessageProcessors
{
	public class ChatTextMessageProcessor : IChatMessageProcessor
	{
		public ChatTextMessageProcessor(IChatEventSender chatEventSender, IJsonSerializer jsonSerializer)
		{
			_chatEventSender = chatEventSender;
			_jsonSerializer = jsonSerializer;
		}

		#region Implementation of IChatMessageProcessor

		public async Task Process(string login, WebSocket webSocket, byte[] requestData)
		{
			var msg = _jsonSerializer.Deserialize<ChatIncomingMessage>(requestData);

			if (msg.Type == ChatMessageType.Public)
			{
				await _chatEventSender.SendPublic(
					new ChatEvent<ChatOutgoingMessage>
					{
						Type = ChatEventType.Message,
						Data = new ChatOutgoingMessage
						{
							Sender = login,
							Type = ChatMessageType.Public,
							Text = msg.Text
						}
					});
			}
			else if (msg.Type == ChatMessageType.Private)
			{
				await _chatEventSender.SendPrivate(
					login,
					msg.Recipient,
					new ChatEvent<ChatOutgoingMessage>
					{
						Type = ChatEventType.Message,
						Data = new ChatOutgoingMessage
						{
							Sender = login,
							Recipient = msg.Recipient,
							Type = ChatMessageType.Private,
							Text = msg.Text
						}
					});
			}
		}

		#endregion

		private readonly IChatEventSender _chatEventSender;
		private readonly IJsonSerializer _jsonSerializer;
	}
}
