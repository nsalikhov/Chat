using System.Net.WebSockets;
using System.Threading.Tasks;



namespace Chat.Infrastructure.MessageProcessors
{
	public interface IChatMessageProcessor
	{
		Task Process(string login, WebSocket webSocket, byte[] requestData);
	}
}
