using Chat.DataContracts;



namespace Chat.Services
{
	public interface IChatEventSender
	{
		void SendPublic<T>(ChatEvent<T> chatEvent);

		void SendPrivate<T>(string recipient, ChatEvent<T> chatEvent);
	}
}
