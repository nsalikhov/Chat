using Newtonsoft.Json;
using Newtonsoft.Json.Converters;



namespace Chat.DataContracts
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum ChatEventType
	{
		Message = 1,
		UsersList = 2
	}
}
