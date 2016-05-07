using System;
using System.Security.Cryptography;
using System.Text;



namespace Chat.Security
{
	public class PasswordConverter : IPasswordConverter
	{
		#region Implementation of IPasswordConverter

		public string GetPasswordHash(string password, string salt)
		{
			using (var hmacSha1 = new HMACSHA1(Encoding.UTF8.GetBytes(salt)))
			{
				var buffer = Encoding.UTF8.GetBytes(password);
				var hash = hmacSha1.ComputeHash(buffer);

				return BitConverter.ToString(hash).ToUpperInvariant();
			}
		}

		#endregion
	}
}
