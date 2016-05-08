namespace Chat.DataContracts
{
	public class ChatOutgoingMessage
	{
		public string Sender { get; set; }

		public string Recipient { get; set; }

		public ChatMessageType Type { get; set; }

		public string Text { get; set; }
	}
}
