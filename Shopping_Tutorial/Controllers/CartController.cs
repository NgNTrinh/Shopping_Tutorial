using Microsoft.AspNetCore.Mvc;
using Shopping_Tutorial.Models.ViewModels;
using Shopping_Tutorial.Models;
using Shopping_Tutorial.Repository;

namespace Shopping_Tutorial.Controllers
{
	public class CartController : Controller
	{
		private readonly DataContext _dataContext;
		public CartController(DataContext _context)
		{
			_dataContext = _context;
		}
		public IActionResult Index()
		{
			//Chứa card Item
			List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
			CartItemViewModel cartVM = new()
			{
				CartItems = cartItems,
				GrandTotal = cartItems.Sum(x => x.Quantity * x.Price)
			};
			return View(cartVM);
		}

		public ActionResult Checkout()
		{
			return View("~/View/Checkout/Index.cshtml");
		}

		//phuong thuc bat dong bo
		public async Task<IActionResult> Add(int Id)
		{
			ProductModel product = await _dataContext.Products.FindAsync(Id);
			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
			CartItemModel cartItems = cart.Where(c => c.ProductId == Id).FirstOrDefault();
			if (cartItems == null)
			{
				cart.Add(new CartItemModel(product));
			}
			else
			{
				cartItems.Quantity += 1;
			}
			//Lưu trữ cart và session cart
			HttpContext.Session.SetJson("Cart", cart);

			TempData["success"] = "Thêm sản phẩm vào giỏ hàng thành công";
			return Redirect(Request.Headers["Referer"].ToString());// tra ve trang hien tai
		}

		public async Task<IActionResult> Decrease(int Id)
		{
			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");
			CartItemModel cartItems = cart.Where(c => c.ProductId == Id).FirstOrDefault();
			if (cartItems.Quantity > 1)
			{
				--cartItems.Quantity;
			}
			else
			{
				cart.RemoveAll(p => p.ProductId == Id);
			}

			if (cart.Count == 0)
			{
				HttpContext.Session.Remove("Cart");
			}
			else
			{
				HttpContext.Session.SetJson("Cart", cart);
			}
			TempData["success"] = "Trừ sản phẩm trong giỏ hàng thành công";
			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Increase(int Id)
		{
			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");
			CartItemModel cartItems = cart.Where(c => c.ProductId == Id).FirstOrDefault();
			if (cartItems.Quantity >= 1)
			{
				++cartItems.Quantity;
			}
			else
			{
				cart.RemoveAll(p => p.ProductId == Id);
			}

			if (cart.Count == 0)
			{
				HttpContext.Session.Remove("Cart");
			}
			else
			{
				HttpContext.Session.SetJson("Cart", cart);
			}
			TempData["success"] = "Cộng sản phẩm trong giỏ hàng thành công";
			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Remove(int Id)
		{
			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");
			// xóa tất cả các sản phẩm trong giỏ hàng có ID trùng khớp với ID sản phẩm được truyền vào.
			cart.RemoveAll(p => p.ProductId == Id);
			if (cart.Count == 0)
			{
				HttpContext.Session.Remove("Cart");
			}
			else
			{
				HttpContext.Session.SetJson("Cart", cart);
			}
			TempData["success"] = "Xóa sản phẩm trong giỏ hàng thành công";
			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Clear()
		{

			HttpContext.Session.Remove("Cart");
			TempData["success"] = "Xóa giỏ hàng thành công";
			return RedirectToAction("Index");
		}
	}
}
