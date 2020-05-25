namespace Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Customer")]
    public partial class Customer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Họ tên không được rỗng")]
        [Display(Name = "Họ tên(*)")]
        public string Name { get; set; }
        [StringLength(250)]
        [Display(Name = "Cơ quan(*)")]
        public string Ogran { get; set; }

        [StringLength(50)]
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        [StringLength(50)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [StringLength(11, ErrorMessage = "Tối đa 11 số")]
        [Required(ErrorMessage = "Chưa nhập số điện thoại")]
        [Range(0, Int64.MaxValue, ErrorMessage = "Điện thoại phải nhập số")]
        [Display(Name = "Điện thoại(*)")]
        public string Phone { get; set; }
        [Display(Name = "Kích hoạt")]
        public bool Status { get; set; }
    }
}
