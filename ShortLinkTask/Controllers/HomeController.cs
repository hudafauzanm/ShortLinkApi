using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ShortLinkTask.Data;
using ShortLinkTask.Model;

namespace ShortLinkTask.Controllers
{
    [Route("/short")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        public IConfiguration Configuration;
        public ApplicationDbContext AppDbContext { get; set; }

        public HomeController(IConfiguration configuration, ApplicationDbContext appDbContext)
        {
            AppDbContext = appDbContext;
            Configuration = configuration;
        }
        // GET: api/Home
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Home/short
        [HttpGet("{url}")]
        public IActionResult Get(string url)
        {
            if (url != null)
            {
                var ip = "";
                var reference = "";
                var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;
                var x = from y in AppDbContext.ShortUrls where y.shorturl == url select y;
                foreach (var link in x)
                {
                    Tracks tracks = new Tracks()
                    {
                        shorturl_id = link.id,
                        referrer_url = link.url,
                        ipaddress = Convert.ToString(remoteIpAddress),
                        created_at = DateTime.Now,
                    };
                    reference = link.url;
                    AppDbContext.Tracks.Add(tracks);
                }
                AppDbContext.SaveChanges();
                return Redirect("http://" + reference);
            }
            return Ok("Url Tidak ada");
        }

        // POST: api/Home
        [HttpPost("{url}")]
        public IActionResult Post(string url)
        {
            if(url != null)
            {
                String hostName = string.Empty;
                hostName = Dns.GetHostName();
                IPHostEntry myIP = Dns.GetHostEntry(hostName);
                Console.WriteLine("=======================================");
                Console.WriteLine(myIP);
                
                var x = from y in AppDbContext.ShortUrls where y.shorturl == url select y;
                foreach(var link in x)
                {
                    Tracks tracks = new Tracks()
                    {
                        shorturl_id = link.id,
                        referrer_url = link.url,
                        ipaddress = Convert.ToString(myIP),
                        created_at = DateTime.Now,
                    };
                    AppDbContext.Tracks.Add(tracks);
                    AppDbContext.SaveChanges();
                    return Redirect(link.url);
                }
            }
            return Ok("Url Tidak ada");
        }

        // PUT: api/Home/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
