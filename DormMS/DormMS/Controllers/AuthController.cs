using DormMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DormMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly DormMsnContext _context;
        private readonly IConfiguration _config;
        public AuthController(DormMsnContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDto dto)
        {
            try
            {
                if (dto == null)
                {
                    Console.WriteLine("DTO NULL");
                    return BadRequest("Không nhận được dữ liệu từ client");
                }
                // Ép chạy validate để ModelState có dữ liệu đầy đủ
                TryValidateModel(dto);
                Console.WriteLine($"ModelState.Count = {ModelState.Count}");
                Console.WriteLine($"ModelState.IsValid = {ModelState.IsValid}");
               

                if (!ModelState.IsValid)
                {
                    Console.WriteLine("ModelState Invalid");
                    return BadRequest(ModelState);
                }

                if (_context.HostelUsers.Any(x => x.Email == dto.Email))
                {
                    Console.WriteLine("Email đã tồn tại");
                    return BadRequest(new
                    {
                        field = "email",
                        message = "Email đã tồn tại"
                    });
                }

                if (!IsStrongPassword(dto.Password))
                {
                    Console.WriteLine("Password không đủ mạnh");
                    return BadRequest(new
                    {
                        field = "password",
                        message = "Mật khẩu phải ≥ 8 ký tự, có chữ hoa, chữ thường và ký tự đặc biệt"
                    });
                }

                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);
                Console.WriteLine(hashedPassword.Length);
                Console.WriteLine($"Hashed password length: {hashedPassword.Length}");

                var user = new HostelUser
                {
                    Username = dto.Username,
                    Name = dto.Username,
                    Email = dto.Email,
                    Password = hashedPassword,
                    Role = 2,
                    Status ="Active"
                };

                Console.WriteLine("Đã tạo object user");

                _context.HostelUsers.Add(user);
                Console.WriteLine("Đã Add vào context");

                _context.SaveChanges();
                Console.WriteLine("SaveChanges thành công");

                return Ok(new { message = "Đăng ký thành công" });
            }
            catch (Exception ex)
            {
                Console.WriteLine("===== LỖI XẢY RA =====");
                Console.WriteLine(ex.ToString());

                return StatusCode(500, ex.ToString());
            }
        }
        private bool IsStrongPassword(string password)
        {
            if (password.Length < 8)
                return false;

            bool hasUpper = password.Any(char.IsUpper);
            bool hasLower = password.Any(char.IsLower);
            bool hasSpecial = password.Any(ch => !char.IsLetterOrDigit(ch));

            return hasUpper && hasLower && hasSpecial;
        }
        [HttpPost("login")]
        public IActionResult Login([FromBody]  LoginDto dto)
        {
            var user = _context.HostelUsers
                .Include(u => u.RoleNavigation)
                .FirstOrDefault(u => u.Email == dto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
                return Unauthorized(new
                {
                    field = "login",    
                    message = "Sai email hoặc mật khẩu"
                });

            var token = GenerateJwt(user);

            return Ok(new
            {
                token,
                fullName = user.Name,
                role = user.RoleNavigation.RoleName
            });
        }
        private string GenerateJwt(HostelUser user)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.RoleNavigation.RoleName)
        };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"])
            );

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
