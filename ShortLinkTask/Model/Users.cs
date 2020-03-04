using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShortLinkTask.Model
{
    public class Users
    {   
        [Key]
        public Guid id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public int role { get; set; }
        public DateTime created_at { get; set; }

    }
}
