using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace e_commerce_web.Data.ViewModel
{
    public class ChangePasswordVM
    {
        public string CustomerId { get; set; }
        [Required]
        public string Passsword { get; set; } = null;

        [Required]
        [MinLength(3, ErrorMessage = "Nhấp tối thiểu 5 ký tự")]
        public string NewPassword { get; set; } = null;

        [Required]
        [MinLength(3, ErrorMessage = "Nhấp tối thiểu 5 ký tự")]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu không khớp")]
        public string ConfirmNewPassword { get; set; } = null;
    }
}
