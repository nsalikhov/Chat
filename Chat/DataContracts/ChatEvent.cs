namespace Chat.DataContracts
{
	public class ChatEvent<T>
	{
		public ChatEventType Type { get; set; }

		public T Data { get; set; }
	}
}
