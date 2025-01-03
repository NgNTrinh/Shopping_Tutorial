using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping_Tutorial.Models;
using Shopping_Tutorial.Repository;

namespace Shopping_Tutorial.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Order")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {

        private readonly DataContext _dataContext;
        public OrderController(DataContext context)
        {
            _dataContext = context;

        }
        [HttpGet]
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.OrderModels.OrderByDescending(p => p.Id).ToListAsync());
        }


        [HttpGet]
        [Route("ViewOrder")]
        public async Task<IActionResult> ViewOrder(string orderCode)
        {
            var DetailsOrder = await _dataContext.OrderDetails
                                                 .Include(od => od.product)
                                                 .Where(od => od.OrderCode == orderCode)
                                                 .ToListAsync();
            return View(DetailsOrder); // Truyền danh sách OrderDetails vào View
        }

        [HttpPost]
        [Route("UpdateOrder")]
        public async Task<IActionResult> UpdateOrder(string orderCode, int status)
        {
            if (string.IsNullOrEmpty(orderCode))
            {
                return BadRequest(new { success = false, message = "Mã đơn hàng không hợp lệ." });
            }

            var order = await _dataContext.OrderModels.FirstOrDefaultAsync(o => o.OrderCode == orderCode);
            if (order == null)
            {
                return NotFound(new { success = false, message = "Không tìm thấy đơn hàng." });
            }

            order.Status = status;

            try
            {
                await _dataContext.SaveChangesAsync();
                return Ok(new { success = true, message = "Cập nhật trạng thái đơn hàng thành công." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi khi cập nhật trạng thái đơn hàng." });
            }
        }
        [HttpGet]
        [Route("DeleteOrder")]
        public async Task<IActionResult> DeleteOrder(int Id)
        {
            OrderModel order = await _dataContext.OrderModels.FindAsync(Id);


            _dataContext.OrderModels.Remove(order);
            await _dataContext.SaveChangesAsync();
            TempData["error"] = "Thương hiệu đã xóa";
            return RedirectToAction("Index");
        }



    }
}
