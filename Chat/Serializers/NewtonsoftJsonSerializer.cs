using System.IO;



namespace Chat.Serializers
{
	public class NewtonsoftJsonSerializer : IJsonSerializer
	{
		public NewtonsoftJsonSerializer()
		{
			_jsonSerializer = new Newtonsoft.Json.JsonSerializer();
		}

		#region Implementation of IJsonSerializer

		public byte[] Serialize(object data)
		{
			using (var stream = new MemoryStream())
			{
				using (var writer = new StreamWriter(stream))
				{
					_jsonSerializer.Serialize(writer, data);
				}

				return stream.ToArray();
			}
		}

		public T Deserialize<T>(byte[] data)
		{
			using (var ms = new MemoryStream(data))
			{
				using (var reader = new StreamReader(ms))
				{
					return (T)_jsonSerializer.Deserialize(reader, typeof(T));
				}
			}
		}

		#endregion

		private readonly Newtonsoft.Json.JsonSerializer _jsonSerializer;
	}
}
