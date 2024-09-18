using System.ComponentModel.DataAnnotations;

namespace PresentaionLayer.ViewModels
{
	public class RegisterViewModel
	{
		[Required]
		public string firstName { get; set; }
		[Required]
		public string lastName { get; set; }
		[Required]
		public string userName { get; set; }
		[EmailAddress]
		public string email { get; set; }
		[DataType(DataType.Password)]
		public string password { get; set; }
		[DataType(DataType.Password)]
		[Compare(nameof(password),ErrorMessage ="the passowrd is wrong")]
		public string ConfirmPassword { get; set; }
		public bool IsAgree { get; set; }


	}
}
