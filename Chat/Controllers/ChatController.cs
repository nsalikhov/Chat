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
		public ChatController(int messageBufferSize)
		{
			_messageBufferSize = messageBufferSize;
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
					}
					while (!receiveResult.EndOfMessage);

					var receivedString = Encoding.UTF8.GetString(ms.ToArray());

					var outputBuffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(receivedString));

					await wsContext.WebSocket.SendAsync(outputBuffer, WebSocketMessageType.Text, true, CancellationToken.None);
				}
			}
		}

		private readonly int _messageBufferSize;
	}
}
