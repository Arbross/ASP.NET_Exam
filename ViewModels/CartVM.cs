using Exam_ASP_NET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_ASP_NET.ViewModels
{
    public class CartVM
    {
        public IEnumerable<Purchase> Purchases { get; set; }
    }
}
