using System.Configuration;



namespace Chat
{
	public static class Settings
	{
		public static int MessageBufferSizeBytes => int.Parse(ConfigurationManager.AppSettings["MessageBufferSizeBytes"]);

		public static int MaxMessageSizeBytes => int.Parse(ConfigurationManager.AppSettings["MaxMessageSizeBytes"]);
	}
}
