using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class RegisterModel
    {
        [Key]
        public long ID { get; set; }
        [Display(Name = "Tên đăng nhập(*)")]
        [Required(ErrorMessage = "Yêu cầu nhập tên đăng nhập")]
        public string UserName { get; set; }
        [Display(Name = "Mật khẩu(*)")]
        [StringLength(20, ErrorMessage = "Độ dài mật khẩu ít nhất 6 kí tự, dài nhất 20 ký tự", MinimumLength = 6)]
        [Required(ErrorMessage = "Yêu cầu nhập mật khẩu")]
        public string Password { get; set; }
        [Display(Name = "Xác nhận mật khẩu(*)")]
        [Compare("Password", ErrorMessage = "Xác nhận mật khẩu không đúng")]
        [Required(ErrorMessage = "Yêu cầu xác nhận mật khẩu")]
        public string ConfirmPassword { get; set; }
        [Display(Name = "Họ tên")]
        [Required(ErrorMessage = "Yêu cầu nhập họ tên")]
        public string Name { get; set; }
        [Display(Name = "Địa chỉ")]
        [Required(ErrorMessage = "Yêu cầu nhập địa chỉ")]
        public string Address { get; set; }
        [Display(Name = "Email(*)")]
        [Required(ErrorMessage = "Yêu cầu nhập email")]
        public string Email { get; set; }
        [Display(Name = "Điện thoại")]
        [Required(ErrorMessage = "Yêu cầu nhập số điện thoại")]
        public string Phone { get; set; }
    }
}
