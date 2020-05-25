namespace Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Contact")]
    public partial class Contact
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte ID { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage ="Nhập tên liên hệ")]
        public string Name { get; set; }

        [Column(TypeName = "ntext")]
        public string Content { get; set; }

        public bool Status { get; set; }
    }
}
