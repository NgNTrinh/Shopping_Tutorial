using Microsoft.EntityFrameworkCore;
using Shopping_Tutorial.Models;

namespace Shopping_Tutorial.Repository
{
	public class SeedData
	{
		public static void SeedingData(DataContext _context)
		{
			_context.Database.Migrate();
			if (!_context.Products.Any())
			{

				CategoryModel apple = new CategoryModel { Name = "Apple", Slug = "apple", Description = "Trumf the gioi", Status = 1 };
				CategoryModel Samsung = new CategoryModel { Name = "Samsung", Slug = "Samsung", Description = "Samsung not trum the world", Status = 1 };

				BrandModel Dell = new BrandModel { Name = "Dell", Slug = "dell", Description = "Trumf the gioi", Status = 1 };
				BrandModel Sony = new BrandModel { Name = "Sony", Slug = "Sony", Description = "Sony not trum the world", Status = 1 };

				_context.Products.AddRange(

					new ProductModel { Name = "Macbook", Slug = "macbook", Description = "Maook the gioi", Image = "1.jpg", Category = apple, Brand = Sony, Price = 1233 },
					new ProductModel { Name = "Pc", Slug = "pc", Description = "PC the gioi", Image = "2.jpg", Category = Samsung, Brand = Dell, Price = 1233 }
				);
				_context.SaveChanges();
			}
		}
	}
}
