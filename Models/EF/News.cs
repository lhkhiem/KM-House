namespace Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("News")]
    public partial class News
    {
        public long ID { get; set; }

        [StringLength(250)]
        [Display(Name="Tiêu đề tin")]
        public string Name { get; set; }

        [StringLength(250)]
        [Display(Name = "Link")]
        public string MetaTitle { get; set; }

        [StringLength(500)]
        [Display(Name = "Mô tả ngắn")]
        public string Description { get; set; }

        [StringLength(250)]
        [Display(Name = "Hình ảnh")]
        public string Image { get; set; }
        [Display(Name = "Danh mục tin")]
        public long NewsCategoryID { get; set; }

        [Column(TypeName = "ntext")]
        [Display(Name = "Chi tiết tin")]
        public string Detail { get; set; }

        public DateTime? CreateDate { get; set; }

        [StringLength(50)]
        public string CreateBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }

        [StringLength(250)]
        public string MetaKeywords { get; set; }

        [StringLength(250)]
        public string MetaDescriptions { get; set; }

        public bool Status { get; set; }

        [StringLength(500)]
        [Display(Name = "Tag SEO")]
        public string Tag { get; set; }
    }
}
