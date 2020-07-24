
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ChatApplicationAuthen.Entities.DTO;
using Microsoft.AspNetCore.Authorization;
using ChatApplicationAuthen.Services;
using ChatApplicationAuthen.Common;
using System.Net;

namespace ChatApplicationAuthen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        #region Field

        private readonly ChatContext _context;
        private IAuthenticateService _authenticateService;

        #endregion

        #region Contructor
        public AuthenticateController(ChatContext context, IAuthenticateService authenticateService)
        {
            _context = context;
            _authenticateService = authenticateService;
        }

        #endregion

        #region Method
        // POST: api/login
        /// <summary>
        ///     Đăng nhập người dùng
        /// </summary>
        /// <param name="userRequest"> Thông tin người dùng đẩy lên từ client </param>
        /// <returns> 
        ///         Trả về dữ liệu gồm code, trạng thái và chi tiết trạng thái  
        ///         Nếu đúng trả về dữ liệu gồm token và user
        /// </returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ResponseResult> Login([FromBody] User userRequest)
        {
            var resultAuthenService = await _authenticateService.Login(userRequest);

            if (resultAuthenService == null)
            {
                return new ResponseResult
                {
                    Code = 1001,
                    Success = false,
                    Message = ErrorConstant.ErrorLogin
                };
            }

            return new ResponseResult
            {
                Code = (int)HttpStatusCode.OK,
                Success = true,
                Message = Constant.SuccessLogin,
                Data = resultAuthenService
            };
        }

        // POST: api/register
        /// <summary>
        ///     Đăng ký người dùng
        /// </summary>
        /// <param name="user"> Thông tin lấy từ request </param>
        /// <returns> 
        ///         Trả về dữ liệu gồm code, trạng thái và chi tiết trạng thái  
        ///         Nếu đúng trả về dữ liệu gồm token và user
        /// </returns>
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ResponseResult> register([FromBody] User user)
        {
            var resultAuthenService = await _authenticateService.Register(user);

            if (resultAuthenService == null)
            {
               return new ResponseResult
                {
                    Code = 1002,
                    Success = false,
                    Message = ErrorConstant.AccountExists
                };
            }
            return new ResponseResult
            {
                Code = (int)HttpStatusCode.OK,
                Success = true,
                Message = Constant.SuccessRegister,
                Data = resultAuthenService
            };
        }
    }
    #endregion
}
