using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace e_commerce_web.Data.ViewModel
{
    public class RegisterVM
    {
        [Key]
        public int CustomerId { get; set; }

        [Display(Name = "Họ Và Tên")]
        [Required(ErrorMessage = "Vui lòng nhập Họ Tên")]
        public string FullName { get; set; }

        [Display(Name = "Ngày sinh nhật")]
        public DateTime? Birthday { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Email")]
        [MaxLength(150)]
        [Remote(action: "ValidateEmail", controller: "Accounts")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Số điện thoại")]
        [MaxLength(11)]
        [Remote(action: "ValidatePhone", controller: "Accounts")]
        public string Phone { get; set; }

 
        [MinLength(5, ErrorMessage = "Bạn cần đặt mật khẩu tôi thiểu 5 char")]
        [Display(Name = "nhập mật khẩu")]
        [Required(ErrorMessage = "Vui nhập mật khẩu")]
        public string Password { get; set; }


        [MinLength(5, ErrorMessage = "Bạn cần đặt mật khẩu tôi thiểu 5 char")]
        [Display(Name = "nhập lại mật khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập confirm mật khẩu")]
        [Compare("Password", ErrorMessage ="Mật khẩu không khớp")]
        public string ConfirmPassword { get; set; }
    }
}
