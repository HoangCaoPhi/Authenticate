using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChatApplicationAuthen.Services;
using ChatApplicationAuthen.Entities.DTO;

namespace ChatApplicationAuthen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        #region Field

        private readonly ChatContext _context;
        private IUserService _userService;

        #endregion

        #region Contructor

        public UsersController(ChatContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }
        #endregion

        #region Method

        /// <summary>
        /// / Hàm lấy ra danh sách các người dùng
        /// </summary>
        /// <returns></returns>
        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }
        /// <summary>
        /// Lấy ra thông tin chi tiết của từng người dùng
        /// </summary>
        /// <param name="id">Id được truyền vào từ client</param>
        /// <returns>Thông tin chi tiết người dùng</returns>
        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }
        /// <summary>
        ///     Cập nhật thông tin của người dùng
        /// </summary>
        /// <param name="id">Id của người dùng</param>
        /// <param name="user">Thông từ được lấy từ client gửi lên</param>
        /// <returns>Thông tin người dùng đã được cập nhật</returns>
        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<ActionResult> PutUser([FromRoute] Guid id, User user)
        {
            try
            {
                var userU = await _userService.update(id, user);
                return Ok(userU);
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/Users/5
      /*  [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }*/

    /*    private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.Id == id);
        }*/
        #endregion
    }
}
