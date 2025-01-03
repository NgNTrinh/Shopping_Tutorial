using System.ComponentModel.DataAnnotations;

namespace Shopping_Tutorial.Models.ViewModels
{
	public class LoginViewModel
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Vui lòng nhập UserName!")]
		public string UserName { get; set; }


		[DataType(DataType.Password), Required(ErrorMessage = "Vui lòng nhập PassWord!")]
		public string Password { get; set; }
		public string ReturnUrl { get; set; }
	}
}
