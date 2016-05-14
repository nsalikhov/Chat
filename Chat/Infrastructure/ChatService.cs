using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;



namespace Chat.Infrastructure
{
	public class ChatService : IChatService
	{
		public IEnumerable<KeyValuePair<string, WebSocket>> Users => UsersInternal;

		#region Implementation of IChatService

		public void AddUser(string login, WebSocket webSocket)
		{
			UsersInternal.AddOrUpdate(login, webSocket, (currentLogin, currentWebSocket) => webSocket);
		}

		public void RemoveUser(string login)
		{
			WebSocket webSocket;

			UsersInternal.TryRemove(login, out webSocket);
		}

		public WebSocket[] GetUsersSockets(string[] logins)
		{
			return UsersInternal
				.Where(x => logins.Contains(x.Key))
				.Select(x => x.Value)
				.ToArray();
		}

		#endregion

		private static readonly ConcurrentDictionary<string, WebSocket> UsersInternal = new ConcurrentDictionary<string, WebSocket>();
	}
}
