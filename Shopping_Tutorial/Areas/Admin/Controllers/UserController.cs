using Microsoft.AspNetCore.Mvc;

namespace Shopping_Tutorial.Areas.Admin.Controllers
{
	public class UserController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
