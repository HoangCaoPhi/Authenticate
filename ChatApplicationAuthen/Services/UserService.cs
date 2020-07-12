using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ChatApplicationAuthen.Helpers;
using ChatApplicationAuthen.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ChatApplicationAuthen.Services
{
    public interface IUserService
    {

        Task<AuthenticateResponse> Login(User user);
        Task<AuthenticateResponse> Register(User user);

    }
    public class UserService : IUserService
    {
        private readonly IConfiguration _config;
        private readonly ChatContext _context;



        public UserService(ChatContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        public async Task<AuthenticateResponse> Login(User userRequest)
        {
            // Lấy thông tin email và password từ request
            var user = await _context.Users
                       .Where(u => u.Email == userRequest.Email
                             && u.Password == userRequest.Password).FirstOrDefaultAsync();

            // Nếu không tìm thấy tài khoản
            if (user == null) return null;

            // Nếu tìm thấy tài khoản trả về token
            var token = generateJwtToken(user);

            // Trả về token và user
            return new AuthenticateResponse(user, token);
        }

        public async Task<AuthenticateResponse> Register(User user)
        {
            // Kiem tra da nhap mat khau chua
            if (string.IsNullOrWhiteSpace(user.Password))
                throw new AppException("Bạn phải nhập mật khẩu !");
            // Kiem tra su ton tai cua email trong csdl
            if (_context.Users.Any(x => x.Email == user.Email))
                throw new AppException("Email " + user.Email + " da ton tai");
            // Them vao luu User
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Nếu tìm thấy tài khoản trả về token
            var token = generateJwtToken(user);

            // Trả về token và user
            return new AuthenticateResponse(user, token);
        }

        public string generateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("userId", user.Id.ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
