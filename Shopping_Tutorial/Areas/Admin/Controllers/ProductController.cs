using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shopping_Tutorial.Models;
using Shopping_Tutorial.Repository;

namespace Shopping_Tutorial.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Route("Admin/Product")]
  	[Authorize(Roles = "Admin")]
    public class ProductController : Controller
	{
		private readonly DataContext _dataContext;
		private readonly IWebHostEnvironment _webHostEnvironment;//Để load file
		public ProductController(DataContext context, IWebHostEnvironment webHostEnvironment)
		{
			_dataContext = context;
			_webHostEnvironment = webHostEnvironment;
		}
        [HttpGet]
        [Route("Index")]
        public async Task<IActionResult> Index()
		{
			return View(await _dataContext.Products.OrderByDescending(p => p.Id).Include(p => p.Category).Include(p => p.Brand).ToListAsync());
		}
        [HttpGet]
        [Route("Create")]
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name");
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name");
            return View();
        }
        [HttpPost]
        [Route("Create")]
    	[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(ProductModel product)
		{
			ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.CategoryId);
			ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandId);

            if (ModelState.IsValid)
            {
                //Thêm data
                product.Slug = product.Name.Replace(" ", "-");
                var slug = await _dataContext.Products.FirstOrDefaultAsync(p => p.Slug == product.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Sản phẩm đã có");
                    return View(product);
                }
                if (product.ImageUpload != null)
                {
                    string upLoadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                    string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                    string filePath = Path.Combine(upLoadDir, imageName);

                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await product.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                    product.Image = imageName;
                }
                _dataContext.Add(product);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Thêm thành công";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Model bị lỗi";
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);
            }
            return View();
		}
        [HttpGet]
        [Route("Edit")]
        public async Task<IActionResult> Edit(int Id)
        {
            ProductModel product = await _dataContext.Products.FindAsync(Id);
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.CategoryId);
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandId);
            return View(product);
        }
        [HttpPost]
        [Route("Edit")]
        [ValidateAntiForgeryToken]
            public async Task<IActionResult> Edit(ProductModel product)
              {
                  ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.CategoryId);
                  ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandId);

                  var edit_product = _dataContext.Products.Find(product.Id);
                  if (ModelState.IsValid)
                  {
                      //Thêm data
                      product.Slug = product.Name.Replace(" ", "-");

                      if (product.ImageUpload != null)
                      {

                          //cập nhật ảnh mới
                          string upLoadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                          string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                          string filePath = Path.Combine(upLoadDir, imageName);


                          //Xóa ảnh củ
                         string oldfileImage = Path.Combine(upLoadDir, edit_product.Image);

                          try
                          {
                              if (System.IO.File.Exists(oldfileImage))
                              {
                                  System.IO.File.Delete(oldfileImage);
                              }

                          }
                          catch (Exception ex)
                          {
                              ModelState.AddModelError("", "Lỗi ");
                          }

                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await product.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                    edit_product.Image = imageName;
                }
                edit_product.Name = product.Name;
                edit_product.Description = product.Description;
                edit_product.Price = product.Price;
                edit_product.CategoryId = product.CategoryId;
                edit_product.BrandId = product.BrandId;


                      _dataContext.Update(edit_product);
                      await _dataContext.SaveChangesAsync();
                      TempData["success"] = "Sửa thành công";
                      return RedirectToAction("Index");
                  }
                  else
                  {
                      TempData["error"] = "Model bị lỗi";
                      List<string> errors = new List<string>();
                      foreach (var value in ModelState.Values)
                      {
                          foreach (var error in value.Errors)
                          {
                              errors.Add(error.ErrorMessage);
                          }
                      }
                      string errorMessage = string.Join("\n", errors);
                      return BadRequest(errorMessage);
                  }
                  return View(product);
              }

        [HttpGet]
        [Route("Delete")]
        public async Task<IActionResult> Delete(int Id)
        {
            ProductModel product = await _dataContext.Products.FindAsync(Id);
            if (product == null)
            {
                return NotFound();
            }

            string upLoadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
            string oldfileImage = Path.Combine(upLoadDir, product.Image);

            try
            {
                if (System.IO.File.Exists(oldfileImage))
                {
                    System.IO.File.Delete(oldfileImage);
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lỗi ");
            }
            _dataContext.Products.Remove(product);
            await _dataContext.SaveChangesAsync();
            TempData["error"] = "Sản phẩm đã xóa";
            return RedirectToAction("Index");
        }
        // thêm số lượng sp
        [Route("AddQuantity")]
        [HttpGet]

        public async Task<IActionResult> AddQuantity(int Id)
        {
            var productbyquantity = await _dataContext.ProductQuantities.Where(pq => pq.ProductId == Id).ToListAsync();
            ViewBag.ProductByQuantity = productbyquantity;
            ViewBag.Id = Id;
            return View();
        }

        [Route("StoreProductQuantity")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult StoreProductQuantity(ProductQuantityModel productQuantityModel)
        {
            var product = _dataContext.Products.Find(productQuantityModel.ProductId);

            if (product == null) { return NotFound();}
            
            product.Quantity += productQuantityModel.Quantity;

            productQuantityModel.Quantity = productQuantityModel.Quantity;
            productQuantityModel.ProductId = productQuantityModel.ProductId;
            productQuantityModel.DateCreated = DateTime.Now;

            _dataContext.Add(productQuantityModel);
            _dataContext.SaveChangesAsync();
            TempData["success"] = "Thêm số lượng sản phẩm thành công";
            return RedirectToAction("AddQuantity", "Product", new {Id = productQuantityModel.ProductId });
        }
    }
}
