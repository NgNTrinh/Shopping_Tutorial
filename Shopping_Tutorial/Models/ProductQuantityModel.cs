﻿using System.ComponentModel.DataAnnotations;

namespace Shopping_Tutorial.Models
{
    public class ProductQuantityModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Yêu cầu không bỏ trống số lượng sp")]

        public int  Quantity { get; set; }

        public int ProductId { get; set; }
        public DateTime DateCreated { get; set; }

    }
}
