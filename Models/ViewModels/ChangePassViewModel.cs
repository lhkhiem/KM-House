using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class ChangePassViewModel
    {

        [StringLength(50)]
        [Required(ErrorMessage = "Nhập email đăng nhập")]
        [Display(Name = "Email đăng nhập")]
        public string Email { get; set; }

        [StringLength(32)]
        [Required(ErrorMessage = "Mật khẩu cũ")]
        [Display(Name = "Mật khẩu cũ")]
        public string Password { get; set; }

        [StringLength(20, ErrorMessage = "Độ dài mật khẩu ít nhất 6 kí tự, dài nhất 20 ký tự", MinimumLength = 6)]
        [Required(ErrorMessage = "Mật khẩu mới")]
        [Display(Name = "Mật khẩu mới")]
        public string NewPassword { get; set; }

        [NotMapped] // Does not effect with your database
        [Required(ErrorMessage = "Xác nhận mật khẩu")]
        [Compare("NewPassword", ErrorMessage = "Xác nhận mật khẩu không đúng")]
        public string ConfirmPassword { get; set; }

    }
}
