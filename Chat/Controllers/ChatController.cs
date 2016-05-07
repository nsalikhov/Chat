using System;
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
		public ActionResult Index()
		{
			return View();
		}

		public Task<EmptyResult> Sync()
		{
			if (HttpContext.IsWebSocketRequest)
			{
				HttpContext.AcceptWebSocketRequest(UserFunc);
			}
			else
			{
				Response.StatusCode = (int)HttpStatusCode.BadRequest;
			}

			return Task.FromResult(new EmptyResult());
		}

		private static async Task UserFunc(AspNetWebSocketContext webSocketContext)
		{
			var buffer = new byte[1024];
			var socket = webSocketContext.WebSocket;

			while (socket.State == WebSocketState.Open)
			{
				await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

				var receivedString = Encoding.UTF8.GetString(buffer);
				var outputBuffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes($"You said: {receivedString}"));

				await socket.SendAsync(outputBuffer, WebSocketMessageType.Text, true, CancellationToken.None);
			}
		}
	}
}
