using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_ASP_NET.Models.ViewModels
{
    public class ViewModel
    {
        // Showing
        public IEnumerable<Purchase> Purchases { get; set; }

        // Create & Edit(Update)
        public Purchase Purchase { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }

        // Search
        public Search Search { get; set; }
    }
}
