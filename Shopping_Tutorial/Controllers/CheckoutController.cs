using Microsoft.AspNetCore.Mvc;

namespace Shopping_Tutorial.Controllers
{
	public class CheckoutController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
