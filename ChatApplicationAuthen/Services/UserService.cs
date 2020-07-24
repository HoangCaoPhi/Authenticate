using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ChatApplicationAuthen.Entities.DTO;

namespace ChatApplicationAuthen.Services
{
 
    public class UserService : IUserService
    {
        #region Field
        private readonly IConfiguration _config;
        private readonly ChatContext _context;
        #endregion

        #region Constructor
        public UserService(ChatContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        #endregion

        #region Method
       
        public async Task<User> update(Guid id, User user)
        {
            var userUpdate = _context.Users.SingleOrDefault(u => u.Id == id);

            if (userUpdate == null) throw new AppException("Không tìm thấy tài khoản update !");
            userUpdate.UserName = user.UserName;
            userUpdate.FirstName = user.FirstName;
            userUpdate.LastName = user.LastName;
            userUpdate.AvatarUrl = user.AvatarUrl;
            userUpdate.ContactMobile = user.ContactMobile;

            _context.Entry(userUpdate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                throw new AppException("Cập nhật thất bại !");
            }
            return userUpdate;
        }   
        #endregion
    }
}
