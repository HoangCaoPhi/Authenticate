
using ChatApplicationAuthen.Entities.DTO;
using ChatApplicationAuthen.Entities.Models;
using System.Threading.Tasks;

namespace ChatApplicationAuthen.Services
{
    public interface IUserService
    {
        /// <summary>
        ///         Đăng nhập người dùng
        /// </summary>
        /// <param name="user"> Thông tin người dùng nhập </param>
        /// <returns> Trả về thông tin người dùng và token </returns>
        Task<AuthenticateResponse> Login(User user);

        /// <summary>
        ///     Đăng ký người dùng
        /// </summary>
        /// <param name="user"> Thông tin người dùng đẩy lên khi đăng ký </param>
        /// <returns> Trả về thông tin người dùng và token </returns>
        Task<AuthenticateResponse> Register(User user);
    }
}
