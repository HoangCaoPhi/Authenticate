using ChatApplicationAuthen.Entities.DTO;
using ChatApplicationAuthen.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ChatApplicationAuthen.Services
{
    public class AuthenticateService : IAuthenticateService
    {
        #region Field
        private readonly IConfiguration _config;
        private readonly ChatContext _context;
        #endregion

        #region Constructor
        public AuthenticateService(ChatContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        #endregion

        #region Method
        /// <summary>
        ///       Mã hóa mật khẩu với MD5
        /// </summary>
        /// <param name="input">
        ///       Đầu vào là một chuỗi mật khẩu của người dùng nhập vào
        /// </param>
        /// <returns>
        ///        Trả về một chuỗi đã được mã hóa
        /// </returns>
        public static string CreateMD5(string input)
        {
            // Lay du lieu de dung md5
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Chuyen doi mang byte thanh chuoi hex
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
        /// <summary>
        ///         Serivce xử lý login vào web
        /// </summary>
        /// <param name="userRequest">
        ///         Đầu vào là request của người dùng được gửi lên
        /// </param>
        /// <returns>
        ///         Trả về đối tượng AuthenticateResponse gồm thông tin người dùng và token nếu đúng và trả về null nếu sai
        /// </returns>
        public async Task<AuthenticateResponse> Login(User userRequest)
        {
            // Lấy thông tin email và password từ request
            var user = await _context.Users
                       .Where(u => u.Email == userRequest.Email
                             && u.Password == CreateMD5(userRequest.Password)).FirstOrDefaultAsync();

            // Nếu không tìm thấy tài khoản
            if (user == null) return null;

            var token = generateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }
        /// <summary>
        ///         Xử lý đăng ký người dùng
        /// </summary>
        /// <param name="user">
        /// </param>
        /// <returns>
        ///   Trả về đối tượng AuthenticateResponse gồm thông tin người dùng và token, trả về message lỗi nếu sai
        /// </returns>
        public async Task<AuthenticateResponse> Register(User user)
        {
            // Kiem tra su ton tai cua email trong csdl
            if (_context.Users.Any(x => x.Email == user.Email))  return null;

            var userAdd = new User
            {
                Id = new Guid(),
                UserName = user.UserName,
                Password = CreateMD5(user.Password),
                FirstName = user.FirstName,
                LastName = user.LastName,
                AvatarUrl = "",
                Email = user.Email,
                ContactMobile = user.ContactMobile
            };

            _context.Users.Add(userAdd);
            await _context.SaveChangesAsync();

            var token = generateJwtToken(userAdd);

            return new AuthenticateResponse(userAdd, token);
        }
        /// <summary>
        ///         SInh ra token để trả về cho người dùng
        /// </summary>
        /// <param name="user"></param>
        /// <returns>
        ///         Trả về token cho người dùng
        /// </returns>
        public string generateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("userId", user.Id.ToString()),
                new Claim("userName", user.UserName.ToString()),
                new Claim("avt", user.AvatarUrl.ToString()),

            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        #endregion

    }
}
