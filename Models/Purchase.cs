using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel.DataAnnotations;

namespace Exam_ASP_NET.Models
{
    public class Purchase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Link { get; set; }

        [Display(Name = "Category")]
        public int? CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}
