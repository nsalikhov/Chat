using System.Net.WebSockets;
using System.Security.Principal;
using System.Threading.Tasks;



namespace Chat.Processors
{
	public interface IChatProcessor
	{
		void AddUser(IIdentity user, WebSocket webSocket);

		void RemoveUser(IIdentity user);

		Task ProcessMessage(WebSocketMessageType messageType, byte[] requestData);
	}
}
