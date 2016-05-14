using System.Collections.Generic;
using System.Net.WebSockets;



namespace Chat.Infrastructure
{
	public interface IChatService
	{
		IEnumerable<KeyValuePair<string, WebSocket>> Users { get; }

		void AddUser(string login, WebSocket webSocket);

		void RemoveUser(string login);

		WebSocket[] GetUsersSockets(string[] logins);
	}
}
