namespace Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Footer")]
    public partial class Footer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte ID { get; set; }
        [StringLength(50)]
        [Required(ErrorMessage = "Nhập tên footer")]
        public string Name { get; set; }

        [Column(TypeName = "ntext")]
        public string Content { get; set; }

        public bool Status { get; set; }
    }
}
