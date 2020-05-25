namespace Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RoomService")]
    public partial class RoomService
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Column(TypeName = "ntext")]
        public string Detail { get; set; }

        public decimal? Price { get; set; }

        [StringLength(10)]
        public string Image { get; set; }

        [StringLength(10)]
        public string MoreImages { get; set; }

        public bool? Status { get; set; }
    }
}
