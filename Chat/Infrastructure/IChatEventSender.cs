using System.Threading.Tasks;

using Chat.DataContracts;



namespace Chat.Infrastructure
{
	public interface IChatEventSender
	{
		Task SendPublic<T>(ChatEvent<T> chatEvent);

		Task SendPrivate<T>(string sender, string recipient, ChatEvent<T> chatEvent);
	}
}
