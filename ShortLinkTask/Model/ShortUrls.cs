using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ShortLinkTask.Model
{
    public class ShortUrls
    {
        public int id { get; set; }
        public string title { get; set; }
        public string shorturl { get; set; }
        public string url { get; set; }
        public string userId { get; set; } 
        public DateTime CreatedAt { get; set; }
    }
}
