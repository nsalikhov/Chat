using System;



namespace Chat.Helpers
{
	public class GuidProvider : IGuidProvider
	{
		#region Implementation of IGuidProvider

		public Guid NewGuid()
		{
			return Guid.NewGuid();
		}

		#endregion
	}
}
