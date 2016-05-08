using Newtonsoft.Json;
using Newtonsoft.Json.Converters;



namespace Chat.DataContracts
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum ChatMessageType
	{
		Public = 1,
		Private = 2
	}
}
