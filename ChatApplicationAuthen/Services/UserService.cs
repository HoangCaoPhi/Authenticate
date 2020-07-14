﻿using System;
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
        ///         Trả về đối tượng AuthenticateResponse gồm thông tin người dùng và token
        /// </returns>
        public async Task<AuthenticateResponse> Login(User userRequest)
        {
            // Lấy thông tin email và password từ request
            var user = await _context.Users
                       .Where(u => u.Email == userRequest.Email
                             && u.Password == CreateMD5(userRequest.Password) ).FirstOrDefaultAsync();

            // Nếu không tìm thấy tài khoản
            if (user == null) return null;

            // Nếu tìm thấy tài khoản trả về token
            var token = generateJwtToken(user);

            // Trả về token và user
            return new AuthenticateResponse(user, token);
        }

        /// <summary>
        ///         Xử lý đăng ký người dùng
        /// </summary>
        /// <param name="user">
        /// </param>
        /// <returns>
        ///   Trả về đối tượng AuthenticateResponse gồm thông tin người dùng và token
        /// </returns>
        public async Task<AuthenticateResponse> Register(User user)
        {
            // Kiem tra da nhap mat khau chua
            if (string.IsNullOrWhiteSpace(user.Password))
                throw new AppException("Bạn phải nhập mật khẩu !");
            // Kiem tra su ton tai cua email trong csdl
            if (_context.Users.Any(x => x.Email == user.Email))
                throw new AppException("Email " + user.Email + " da ton tai");

            // Tao user voi request 
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
            // Them vao luu User
            _context.Users.Add(userAdd);
            await _context.SaveChangesAsync();

            // Nếu tìm thấy tài khoản trả về token
            var token = generateJwtToken(userAdd);

            // Trả về token và user
            return new AuthenticateResponse(userAdd, token);
        }

        /// <summary>
        ///         SInh ra token để trả về cho người dùng
        /// </summary>
        /// <param name="user"></param>
        /// <returns>
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
    }
}
