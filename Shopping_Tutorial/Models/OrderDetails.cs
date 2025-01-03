using System.ComponentModel.DataAnnotations.Schema;

namespace Shopping_Tutorial.Models
{
	public class OrderDetails
	{
		public int Id { get; set; }
		public string UserName { get; set; }
		public string OrderCode { get; set; }
		public int ProductId { get; set; }
		public decimal Gia { get; set; }
		public int soLuong { get; set; }
		[ForeignKey("ProductId")]
		public ProductModel product { get; set; }
	}
}
