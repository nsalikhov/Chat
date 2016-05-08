namespace Chat.DataContracts
{
	public class ChatIncomingMessage
	{
		public string Recipient { get; set; }

		public ChatMessageType Type { get; set; }

		public string Text { get; set; }
	}
}
