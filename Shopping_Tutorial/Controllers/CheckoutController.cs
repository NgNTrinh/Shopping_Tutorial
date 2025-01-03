using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping_Tutorial.Models;
using Shopping_Tutorial.Repository;
using System.Security.Claims;

namespace Shopping_Tutorial.Controllers
{
	public class CheckoutController : Controller
	{
		private readonly DataContext _dataContext;
		public CheckoutController(DataContext dataContext)
		{
			_dataContext = dataContext;
		}
		public async Task<IActionResult> Checkout()
		{
			var userEmail = User.FindFirstValue(ClaimTypes.Email);//timf phien đăng nhập userEmail
			if (userEmail == null)
			{
				return RedirectToAction("Login","Account");

			}
			else
			{
				var orderCode = Guid.NewGuid().ToString();// tạo ra chuỗi đơn hàng
				var orderIten = new OrderModel();
				orderIten.OrderCode = orderCode;
				orderIten.UserName = userEmail;
				orderIten.Status = 1;
				orderIten.CreatedDate = DateTime.Now;
				_dataContext.Add(orderIten);
				_dataContext.SaveChanges();
				//Lưu nhiều sản phẩm vao 1 cái session Cart
				List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
				foreach (var cart in cartItems)
				{
					var orderDetails = new OrderDetails();
					orderDetails.UserName = userEmail;
					orderDetails.OrderCode = orderCode;
					orderDetails.ProductId = (int)cart.ProductId;
					orderDetails.Gia = cart.Price;
					orderDetails.soLuong = cart.Quantity;
					//update số lượng sp
					var product = await _dataContext.Products.Where(p => p.Id == cart.ProductId).FirstAsync();
					product.Quantity -= cart.Quantity;
					product.Sold += cart.Quantity;
					_dataContext.Update(product);


					_dataContext.Add(orderDetails);
					_dataContext.SaveChanges();
				}
				//Xóa session cart
				HttpContext.Session.Remove("Cart");
				TempData["success"] = "Đơn hàng đã được tạo";
				return RedirectToAction("Index", "Cart");
			}
			return View();
		}
	}
}
