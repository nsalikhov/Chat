using System.Collections.Generic;
using System.Net.WebSockets;



namespace Chat.Chat
{
	public interface IChatService
	{
		IEnumerable<KeyValuePair<string, WebSocket>> Users { get; }

		void AddUser(string login, WebSocket webSocket);

		void RemoveUser(string login);

		bool TryGetUserSocket(string login, out WebSocket webSocket);
	}
}
