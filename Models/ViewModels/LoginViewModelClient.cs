using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class LoginViewModelClient
    {
        [Key]
        [Display(Name = "Email đăng nhập(*)")]
        [Required(ErrorMessage = "Bạn phải nhập Email")]
        public string Email { get; set; }
        [Display(Name = "Mật khẩu(*)")]
        [Required(ErrorMessage = "Bạn phải nhập mật khẩu")]
        public string Password { get; set; }
    }
}
