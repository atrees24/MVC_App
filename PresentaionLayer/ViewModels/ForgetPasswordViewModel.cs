using System.ComponentModel.DataAnnotations;

namespace PresentaionLayer.ViewModels
{
	public class ForgetPasswordViewModel
	{
		[EmailAddress]
        public string Email { get; set; }
    }
}
