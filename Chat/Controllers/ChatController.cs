using System;
using System.IO;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.WebSockets;



namespace Chat.Controllers
{
	[Authorize]
	public class ChatController : Controller
	{
		public ChatController(int messageBufferSize, int maxMessageSize)
		{
			_messageBufferSize = messageBufferSize;
			_maxMessageSize = maxMessageSize;
		}

		public ActionResult Index()
		{
			return View();
		}

		public Task<EmptyResult> Sync()
		{
			if (HttpContext.IsWebSocketRequest)
			{
				HttpContext.AcceptWebSocketRequest(ProcessRequest);
			}
			else
			{
				Response.StatusCode = (int)HttpStatusCode.BadRequest;
			}

			return Task.FromResult(new EmptyResult());
		}

		private async Task ProcessRequest(AspNetWebSocketContext wsContext)
		{
			var buffer = new ArraySegment<byte>(new byte[_messageBufferSize]);

			while (wsContext.WebSocket.State == WebSocketState.Open)
			{
				using (var ms = new MemoryStream())
				{
					WebSocketReceiveResult receiveResult;

					do
					{
						receiveResult = await wsContext.WebSocket.ReceiveAsync(buffer, CancellationToken.None);

						await ms.WriteAsync(buffer.Array, 0, receiveResult.Count);

						if (ms.Length >= _maxMessageSize)
						{
							await wsContext.WebSocket.CloseAsync(WebSocketCloseStatus.MessageTooBig, string.Empty, CancellationToken.None);

							return;
						}
					}
					while (!receiveResult.EndOfMessage);

					if (receiveResult.MessageType == WebSocketMessageType.Close)
					{
						await wsContext.WebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);

						return;
					}

					if (receiveResult.MessageType != WebSocketMessageType.Text)
					{
						await wsContext.WebSocket.CloseAsync(WebSocketCloseStatus.InvalidMessageType, string.Empty, CancellationToken.None);

						return;
					}

					var receivedString = Encoding.UTF8.GetString(ms.ToArray());

					var outputBuffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(receivedString));

					await wsContext.WebSocket.SendAsync(outputBuffer, WebSocketMessageType.Text, true, CancellationToken.None);
				}
			}
		}

		private readonly int _messageBufferSize;
		private readonly int _maxMessageSize;
	}
}
