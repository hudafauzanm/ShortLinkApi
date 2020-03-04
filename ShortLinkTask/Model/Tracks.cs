using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShortLinkTask.Model
{
    public class Tracks
    {
        public Guid Id { get; set; }
        public int shorturl_id { get; set; }
        public string ipaddress { get; set; }
        public string referrer_url { get; set; }
        public DateTime created_at { get; set; }
    }
}
