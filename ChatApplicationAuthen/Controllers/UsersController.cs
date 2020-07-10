using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChatApplicationAuthen.Models;
using Microsoft.AspNetCore.Authorization;
using ChatApplicationAuthen.Helpers;
using Microsoft.Extensions.Options;
using ChatApplicationAuthen.Services;
using Microsoft.AspNetCore.Cors;

namespace ChatApplicationAuthen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ChatContext _context;
        private readonly JWTSettings _jwtsettings;
        private IUserService _userService;
    

        public UsersController(ChatContext context, IOptions<JWTSettings> jwtsettings, IUserService userService)
        {
            _context = context;
            _jwtsettings = jwtsettings.Value;
            _userService = userService;
        }
        // POST: api/login
        [AllowAnonymous]
        [HttpPost("login")]
 
        public async Task<ActionResult<AuthenticateResponse>> Login([FromBody] LoginRequest loginRequest)
        {
            var resultAuthenService = await _userService.Login(loginRequest);

            if (resultAuthenService == null) return BadRequest(new { message = "Tài khoản hoặc mật khẩu không chính xác !" });


            return Ok(resultAuthenService);
        }

        // POST: api/register
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<AuthenticateResponse>> register([FromBody] User user)
        {
            try
            {
                // create user
                var resultAuthenService = await _userService.Register(user);
                return Content("Ban da tao tai khoan thanh cong");
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
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
    }
}
