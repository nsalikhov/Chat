using System.ComponentModel.DataAnnotations;



namespace Chat.Models
{
	public class SignInModel
	{
		[Required]
		public string Login { get; set; }

		[Required]
		public string Password { get; set; }

		public bool RememberMe { get; set; }
	}
}
