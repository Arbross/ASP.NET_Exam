using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_ASP_NET.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
    }
}
