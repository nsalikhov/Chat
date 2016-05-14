using System;
using System.IO;
using System.Net;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.WebSockets;

using Chat.Infrastructure;



namespace Chat.Controllers
{
	[Authorize]
	public class ChatController : Controller
	{
		public ChatController(int messageBufferSize, int maxMessageSize, IChatProcessor chatProcessor)
		{
			_messageBufferSize = messageBufferSize;
			_maxMessageSize = maxMessageSize;
			_chatProcessor = chatProcessor;
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
			_chatProcessor.AddUser(User.Identity, wsContext.WebSocket);

			var buffer = new ArraySegment<byte>(new byte[_messageBufferSize]);

			try
			{
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
								throw new InvalidOperationException("Message size too big.");
							}
						}
						while (!receiveResult.EndOfMessage);

						await _chatProcessor.ProcessMessage(User.Identity, wsContext.WebSocket, receiveResult.MessageType, ms.ToArray());
					}
				}
			}
			catch (Exception)
			{
				await wsContext.WebSocket.CloseAsync(WebSocketCloseStatus.InternalServerError, string.Empty, CancellationToken.None);
			}

			_chatProcessor.RemoveUser(User.Identity);
		}

		private readonly IChatProcessor _chatProcessor;
		private readonly int _maxMessageSize;
		private readonly int _messageBufferSize;
	}
}
