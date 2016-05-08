using System.Configuration;



namespace Chat
{
	public static class Settings
	{
		public static int MessageBufferSizeBytes => int.Parse(ConfigurationManager.AppSettings["MessageBufferSizeBytes"]);
	}
}
