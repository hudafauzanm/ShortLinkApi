using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ShortLinkTask.Data;
using ShortLinkTask.Model;

namespace ShortLinkTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        public IConfiguration Configuration;
        public ApplicationDbContext AppDbContext { get; set; }

        public LoginController(IConfiguration configuration, ApplicationDbContext appDbContext)
        {
            AppDbContext = appDbContext;
            Configuration = configuration;
        }

        // GET: api/Login
        [HttpGet]
        public IActionResult Get()
        {
            var get = HttpContext.Session.GetString("JWTToken");
            var getid = HttpContext.Session.GetString("Id");
            Console.WriteLine(get);
            Console.WriteLine(getid);
            return Ok(new { token = get,id =  getid });
        }

        // POST: api/Login/Registrasi
        [HttpPost("Registrasi")]
        public IActionResult Registrasi(Users users)
        {
            var email = from e in AppDbContext.Users select e.email;
            foreach(var e in email)
            {
                if(e == users.email)
                {
                    return Ok(new { data = "Emailmu sudah dipakai" });
                }
                string mysalt = BCrypt.Net.BCrypt.GenerateSalt();
                var BPassword = BCrypt.Net.BCrypt.HashPassword(users.password, mysalt);
                Users User = new Users()
                {
                    username = users.username,
                    email = users.email,
                    password = BPassword,
                    role = users.role,
                    created_at = DateTime.Now,
                };
                AppDbContext.Users.Add(User);
            }
            AppDbContext.SaveChanges();
            return Ok(User);
        }

        // POST: api/Login
        [HttpPost]
        public IActionResult Post(Users users)
        {
            var user = AuthenticatedUser(users.email,users.password);
            if (user != null)
            {
                if (user.role == 1)
                {
                    var token = GenerateJwtToken(user);
                    return Ok(new { data = token,id = user.id.ToString(), role = "1" });
                }
                if (user.role == 2)
                {
                    var token = GenerateJwtToken(user);
                    return Ok(new { data = token, id = user.id.ToString(), role ="2" });
                }
            }
            return Ok(new { data = "User Belum Ada" });
        }

        // PUT: api/Login/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        public string GenerateJwtToken(Users users)
        {
            var secuityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(secuityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                // new Claim(JwtRegisteredClaimNames.Sub,user.username),
                new Claim(JwtRegisteredClaimNames.Sub,Convert.ToString(users.email)),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: Configuration["Jwt:Issuer"],
                audience: Configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: credentials);

            var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);

            return encodedToken;
        }

        private Users AuthenticatedUser(string Email, string Password)
        {
            Users user = null;
            var x = (from data in AppDbContext.Users where data.email == Email orderby data.id select new { data.username, data.password, data.id, data.role, data.email }).LastOrDefault();
            var verify = BCrypt.Net.BCrypt.Verify(Password, x.password);
            if (x.email == Email && (verify == true))
            {
                user = new Users
                {
                    id = x.id,
                    role = x.role,
                    email = x.email,
                    password = x.password,
                };
            }
            return user;
        }
    }
}
