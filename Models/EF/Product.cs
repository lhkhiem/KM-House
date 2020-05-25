namespace Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Product")]
    public partial class Product
    {
        public long ID { get; set; }

        [StringLength(250)]
        [Required(ErrorMessage = "Chưa có tên sản phẩm")]
        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; }

        [StringLength(20)]
        [Display(Name = "Mã code")]
        public string Code { get; set; }

        [StringLength(250)]
        [Display(Name = "Tiêu đề")]
        public string MetaTitle { get; set; }

        [Column(TypeName = "ntext")]
        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [StringLength(250)]
        [Display(Name = "Hình sản phẩm(500x500px)")]
        public string Image { get; set; }

        [Column(TypeName = "xml")]
        [Display(Name = "Kho ảnh sản phẩm")]
        public string MoreImage { get; set; }
        [Display(Name = "Giá")]
        public decimal? Price { get; set; }
        [Display(Name = "Giá giảm")]
        public decimal? PromotionPrice { get; set; }
        [Display(Name = "Đơn vị")]
        public byte UnitID { get; set; }
        [Display(Name = "Số lượng")]
        public byte? Quantity { get; set; }

        [Column(TypeName = "ntext")]
        [Display(Name = "Chi tiết")]
        public string Detail { get; set; }
        [Display(Name = "Bảo hành")]
        public int? Warranty { get; set; }
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

        [StringLength(250)]
        [Display(Name = "Từ khóa SEO")]
        public string MetaKeywords { get; set; }

        [StringLength(250)]
        [Display(Name = "Mô tả SEO")]
        public string MetaDescriptions { get; set; }
        [Display(Name = "Trạng thái")]
        public bool Status { get; set; }
        [Display(Name = "Hiện trang chủ")]
        public bool ShowOnHome { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Tot Hot")]
        public DateTime? TopHot { get; set; }

        public bool? ViewCount { get; set; }
        [Display(Name = "Danh mục")]
        public long ProductCategoryID { get; set; }
        [StringLength(500)]
        [Display(Name = "Tag SEO")]
        public string Tag { get; set; }
    }
}
