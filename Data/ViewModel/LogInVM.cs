using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace e_commerce_web.Data.ViewModel
{
    public class LogInVM
    {
        [Key]
        [MaxLength(100)]
        [Required(ErrorMessage ="Vui lòng nhập")]
        [Display(Name ="Nhập Username")]
        public string UserName { get; set; }
        
        [Display(Name = "Nhập Mật khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập")]
        [MinLength(5, ErrorMessage = "Nhấp tối thiểu 5 ký tự")]
        public string Password { get; set; }
    }
}
