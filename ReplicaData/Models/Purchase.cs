using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exam_ASP_NET.Models
{
    public enum Status
    {
        Pending,
        Proccessing,
        Completed
    }

    public class Purchase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Link { get; set; }
        public Status Status { get; set; }

        [Display(Name = "Category")]
        public int? CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}
