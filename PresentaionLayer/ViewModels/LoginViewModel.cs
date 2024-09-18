using System.ComponentModel.DataAnnotations;

namespace PresentaionLayer.ViewModels
{
	public class LoginViewModel
	{
		[Required]
		[EmailAddress]
		public string email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string password { get; set; }
        public bool RememberMe { get; set; }
    }
}
