
using ChatApplicationAuthen.Entities.DTO;
using ChatApplicationAuthen.Entities.Models;
using System;
using System.Threading.Tasks;

namespace ChatApplicationAuthen.Services
{
    public interface IUserService
    {
        /// <summary>
        ///     Cập nhật thông tin người dùng
        /// </summary>
        /// <param name="id">Id của người dùng</param>
        /// <param name="user">Thông tin lấy từ request của client</param>
        /// <returns></returns>
        Task<User> update(Guid id, User user);
    }
}
