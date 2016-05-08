using System.Net.WebSockets;
using System.Threading.Tasks;



namespace Chat.Chat.MessageProcessors
{
	public interface IChatMessageProcessor
	{
		Task Process(string login, WebSocket webSocket, byte[] requestData);
	}
}
