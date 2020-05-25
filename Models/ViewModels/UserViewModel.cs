using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class UserViewModel
    {
        public long ID { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Tên đăng nhập")]
        [Display(Name = "Tên đăng nhập")]
        public string UserName { get; set; }

        [StringLength(32)]
        [Required(ErrorMessage = "Mật khẩu")]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [NotMapped] // Does not effect with your database
        [Required(ErrorMessage = "Xác nhận mật khẩu")]
        [Compare("Password", ErrorMessage = "Xác nhận mật khẩu không đúng")]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Nhóm tài khoản")]
        public string UserGroupID { get; set; }
        public string UserGroup { get; set; }
        [StringLength(50)]
        [Display(Name = "Họ tên")]
        [Required(ErrorMessage = "Họ tên người dùng")]
        public string Name { get; set; }

        [StringLength(50)]
        [Display(Name = "Địa chỉ")]
        [Required(ErrorMessage = "Địa chỉ")]
        public string Address { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Địa chỉ Email")]
        public string Email { get; set; }

        [StringLength(50)]
        [Display(Name = "Điện thoại")]
        [Required(ErrorMessage ="Số điện thoại")]
        public string Phone { get; set; }
        [Display(Name = "Ngày tạo")]
        public DateTime? CreateDate { get; set; }

        [StringLength(50)]
        [Display(Name = "Người tạo")]
        public string CreateBy { get; set; }
        [Display(Name = "Ngày sửa")]
        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        [Display(Name = "Người sửa")]
        public string ModifiedBy { get; set; }
        [Display(Name = "Kích hoạt")]
        public bool Status { get; set; }
    }
}
