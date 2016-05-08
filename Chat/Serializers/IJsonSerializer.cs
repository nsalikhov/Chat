namespace Chat.Serializers
{
	public interface IJsonSerializer
	{
		byte[] Serialize(object data);

		T Deserialize<T>(byte[] data);
	}
}
