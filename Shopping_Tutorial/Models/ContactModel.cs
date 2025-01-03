﻿using CK_ASP_NET_CORE.Repository.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shopping_Tutorial.Models
{
    public class ContactModel


    {

        [Key]


        [Required(ErrorMessage = "Yêu cầu nhập tiêu đề Website")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập số điện thoại")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập Mô tả")]
        public string Description { get; set; }

        public string LogoImg { get; set; }

        [NotMapped]
        [KiemTra]
        public IFormFile? ImageUpload { get; set; }
    }
}
