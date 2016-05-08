using System.Net.WebSockets;
using System.Security.Principal;
using System.Threading.Tasks;



namespace Chat.Chat
{
	public interface IChatProcessor
	{
		void AddUser(IIdentity user, WebSocket webSocket);

		void RemoveUser(IIdentity user);

		Task ProcessMessage(IIdentity user, WebSocket webSocket, WebSocketMessageType messageType, byte[] requestData);
	}
}
