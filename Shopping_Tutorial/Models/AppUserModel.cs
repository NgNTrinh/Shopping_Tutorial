using Microsoft.AspNetCore.Identity;

namespace Shopping_Tutorial.Models
{
	public class AppUserModel : IdentityUser
	{
		public string ngheNghiep { get; set; }
	}
}
