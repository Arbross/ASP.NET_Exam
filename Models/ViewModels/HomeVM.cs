﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_ASP_NET.Models.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Purchase> Purchases { get; set; }
    }
}
