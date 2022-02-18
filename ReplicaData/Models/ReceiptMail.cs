using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_ASP_NET.Models
{
    public class ReceiptMail
    {
        public IEnumerable<Purchase> Purchases { get; set; }
        public string UserName { get; set; }
    }
}
