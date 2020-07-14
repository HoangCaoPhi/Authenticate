using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using ChatApplicationAuthen.Helpers;
using Microsoft.Extensions.Options;
using ChatApplicationAuthen.Services;
using ChatApplicationAuthen.Entities.DTO;
using ChatApplicationAuthen.Entities.Models;

namespace ChatApplicationAuthen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        #region Field

        private readonly ChatContext _context;
        private readonly JWTSettings _jwtsettings;
        private IUserService _userService;

        #endregion

        #region Contructor

        public UsersController(ChatContext context, IOptions<JWTSettings> jwtsettings, IUserService userService)
        {
            _context = context;
            _jwtsettings = jwtsettings.Value;
            _userService = userService;
        }

        #endregion

        #region Method
        // POST: api/login
        /// <summary>
        ///     Đăng nhập người dùng
        /// </summary>
        /// <param name="userRequest"> Thông tin người dùng đẩy lên từ client </param>
        /// <returns> Trả về thông tin người dùng và token nếu đúng không trả thông báo lỗi </returns>
        [AllowAnonymous]
        [HttpPost("login")]

        public async Task<ActionResult<AuthenticateResponse>> Login([FromBody] User userRequest)
        {
            var resultAuthenService = await _userService.Login(userRequest);

            if (resultAuthenService == null) return BadRequest(new { message = "Email hoặc mật khẩu không chính xác !" });

            return Ok(resultAuthenService);
        }

        // POST: api/register
        /// <summary>
        ///     Đăng ký người dùng
        /// </summary>
        /// <param name="user"> Thông tin lấy từ request </param>
        /// <returns> Trả về thông tin người dùng và token nếu đăng ký thành công không thì trả về thông báo </returns>
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<AuthenticateResponse>> register([FromBody] User user)
        {
            try
            {
                var resultAuthenService = await _userService.Register(user);
                return Ok(resultAuthenService);
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        // GET: api/Users
 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

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

        // PUT: api/Users/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(Guid id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }




        // DELETE: api/Users/5
        [HttpDelete("{id}")]
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
        }

        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
        #endregion
    }
}
