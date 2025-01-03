using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping_Tutorial.Models;
using Shopping_Tutorial.Repository;

namespace Shopping_Tutorial.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/AppRoles")]
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
	{
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly DataContext _dataContext;
        public RoleController(DataContext context, RoleManager<IdentityRole> roleManager)
        {

            _dataContext = context;
            _roleManager = roleManager;

        }
        [Route("Index")]
        public async Task<IActionResult> Index()
		{
			return View(await _dataContext.Roles.OrderByDescending(p =>p.Id).ToListAsync());
		}
        [HttpGet]
        [Route("Create")]
        public IActionResult Create() {
            return View();
        }
        [HttpGet]
        [Route("Edit")]
        public async Task<IActionResult> Edit(string id)
        {
            if(string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            var role = await _roleManager.FindByIdAsync(id);
            return View(role);
        }

        [HttpPost]
        [Route("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, IdentityRole model)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(id);
                if (role == null)
                {
                    return NotFound();
                }
                role.Name = model.Name;

                try
                {
                    await _roleManager.UpdateAsync(role);
                    TempData["success"] = "Sửa quyền thành công";
                    return RedirectToAction("Index");
                }catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi sửa quyền");
                }
            }
            return View(model ?? new IdentityRole { Id = id});
        }
            [HttpPost]
        [Route("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IdentityRole model)
        {
            if(!_roleManager.RoleExistsAsync(model.Name).GetAwaiter().GetResult()) 
            {
            _roleManager.CreateAsync(new IdentityRole(model.Name)).GetAwaiter().GetResult();
            }
            return Redirect("Index");
        }

        [HttpGet]
        [Route("Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();  
            }
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();  
            }
            try
            {
                await _roleManager.DeleteAsync(role);
                TempData["success"] = "Xóa quyền thành công!";
            }
             catch (Exception ex)
            {
                ModelState.AddModelError("", "Xảy ra lỗi");
            }
            return Redirect("Index");
        }

    }
}
