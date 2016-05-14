using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;



namespace Chat.Infrastructure.MessageProcessors
{
	public class ChatCloseMessageProcessor : IChatMessageProcessor
	{
		#region Implementation of IChatMessageProcessor

		public async Task Process(string login, WebSocket webSocket, byte[] requestData)
		{
			await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
		}

		#endregion
	}
}
