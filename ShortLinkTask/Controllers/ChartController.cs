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
    [Route("/chart")]
    [ApiController]
    public class ChartController : ControllerBase
    {
        public IConfiguration Configuration;
        public ApplicationDbContext AppDbContext { get; set; }

        public ChartController(IConfiguration configuration, ApplicationDbContext appDbContext)
        {
            AppDbContext = appDbContext;
            Configuration = configuration;
        }

        // GET: api/Chart/5
        [HttpPost("Search")]
        public IActionResult Search(ShortUrls shorturl)
        {
            if(shorturl.shorturl != null)
            {
                var shortid = from s in AppDbContext.ShortUrls where s.shorturl == shorturl.shorturl select s.id;
                var id = 0;
                foreach(var s in shortid)
                {
                    id = s;
                }
                var count = (from t in AppDbContext.Tracks where t.shorturl_id == id select t).Count();
                var getip = (from t in AppDbContext.Tracks where t.shorturl_id == id select t.ipaddress).Distinct();
                return Ok(new { data = count, ip = getip });
            }
            return Ok(new { data = "MASUKKAN SHORT URL!!!!" });
        }

        // POST: api/Chart
        [HttpPost]
        public IActionResult Post(Chart charts)
        {
            Console.WriteLine(charts.filter + charts.shorturl);
            var shortid = from s in AppDbContext.ShortUrls where s.shorturl == charts.shorturl select s.id;
            var id = 0;
            foreach (var s in shortid)
            {
                id = s;
            }
            if (charts.filter == "day")
            {
                var day = from d in AppDbContext.Tracks where d.shorturl_id == id group d by d.created_at.Date into g select new { Hari = g.Key.ToString(), Count = g.Count() };
                foreach(var d in day)
                {
                    Console.WriteLine(Convert.ToString(d.Hari) + d.Count);
                    if(d.Hari != null)
                    {
                        Console.WriteLine("masuk sini count 0");
                        return Ok(new { label = d.Hari, value = d.Count });
                    }
                    return Ok(new { label = 0, value = 0 });
                }
            }
            if (charts.filter == "month")
            {
                var day = from d in AppDbContext.Tracks where d.shorturl_id == id group d by d.created_at.Month into g select new { Hari = g.Key.ToString(), Count = g.Count() };
                foreach (var d in day)
                {
                    Console.WriteLine(Convert.ToString(d.Hari), d.Count);
                    if (d.Count == 0)
                    {
                        return Ok(new { label = 0, value = 0 });
                    }
                    return Ok(new { label = d.Hari, value = d.Count });
                }
            }
            if (charts.filter == "year")
            {
                var day = from d in AppDbContext.Tracks where d.shorturl_id == id group d by d.created_at.Year into g select new { Hari = g.Key.ToString(), Count = g.Count() };
                foreach (var d in day)
                {
                    Console.WriteLine(Convert.ToString(d.Hari), d.Count);
                    if (d.Count == 0)
                    {
                        return Ok(new { label = 0, value = 0 });
                    }
                    return Ok(new { label = d.Hari, value = d.Count });
                }
            }
            return Ok("jembluk");
        }

        // PUT: api/Chart/5
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

    public class Chart
    {
        public string filter { get; set; }
        public string shorturl { get; set; }
    }
}
