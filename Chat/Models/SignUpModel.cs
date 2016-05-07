using System.ComponentModel.DataAnnotations;



namespace Chat.Models
{
	public class SignUpModel
	{
		[Required]
		public string Login { get; set; }

		[Required]
		public string Password { get; set; }
	}
}
