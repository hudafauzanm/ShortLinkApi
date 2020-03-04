using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ShortLinkTask.Data;
using ShortLinkTask.Model;

namespace ShortLinkTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShortLinkController : ControllerBase
    {
        public IConfiguration Configuration;
        public ApplicationDbContext AppDbContext { get; set; }

        public ShortLinkController(IConfiguration configuration, ApplicationDbContext appDbContext)
        {
            AppDbContext = appDbContext;
            Configuration = configuration;
        }
        // GET: api/ShortLink
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/ShortLink/5
        [HttpGet("{id}", Name = "ShortLink")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/ShortLink
        [HttpPost]
        public IActionResult Post(ShortUrls shorturls)
        {
            var random = RandomString();
            if(shorturls.shorturl == null)
            {
                ShortUrls shortUrls = new ShortUrls()
                {
                    title = shorturls.url,
                    shorturl = random,
                    url = shorturls.url,
                    userId = shorturls.userId,
                    CreatedAt = DateTime.Now
                };
                AppDbContext.ShortUrls.Add(shortUrls);
            }
            else
            {
                Console.WriteLine(shorturls.shorturl);
                var hasil = from x in AppDbContext.ShortUrls where x.shorturl == shorturls.shorturl select x;
                foreach(var x in hasil)
                {
                    if (x.shorturl != null)
                    {
                        return Ok(new { data = "Short URL sudah dipakai!!!" });
                    }
                }
                ShortUrls shortUrl = new ShortUrls()
                {
                    title = shorturls.url,
                    shorturl = shorturls.shorturl,
                    url = shorturls.url,
                    userId = shorturls.userId,
                    CreatedAt = DateTime.Now
                };
                AppDbContext.ShortUrls.Add(shortUrl);
                AppDbContext.SaveChanges();
                return Ok( new { data = "https://192.168.17.39:7001/short/" + shorturls.shorturl });
            }
            AppDbContext.SaveChanges();
            return Ok(new { data = "https://192.168.17.39:7001/short/" + random });
        }

        // PUT: api/ShortLink/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        private static Random random = new Random();
        public static string RandomString()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, 6)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
