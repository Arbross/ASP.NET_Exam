using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Exam_ASP_NET.Utilities
{
    public class AlertData
    {
        public string Text { get; set; }
        public string Type { get; set; }

        public AlertData() { }
        public AlertData(string jsonString)
        {
            var data = JsonSerializer.Deserialize<AlertData>(jsonString);
            this.Text = data.Text;
            this.Type = data.Type;
        }
    }
}