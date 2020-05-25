using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class NewsViewModel
    {
        public long ID { get; set; }
        public string Name { get; set; }

        public string MetaTitle { get; set; }

        public string Description { get; set; }
        public string Image { get; set; }

        public long NewsCategoryID { get; set; }
        public string Detail { get; set; }

        public DateTime? CreateDate { get; set; }
        public string CreateBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescriptions { get; set; }

        public bool Status { get; set; }
        public string Tag { get; set; }
        public string NewsCategoryName { get; set; }
        public string NewsCategoryMetaTitle { get; set; }
    }
}
