using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApplicationAuthen.Common
{
    public class ErrorConstant
    {
        public static string NotFoundCustomerMsg = "Không tìm thấy tài khoản.";
        public static string CreateFailMsg = "Tạo tài khoản thất bại.";
        public static string ModelStateInvalidMsg = "Request chứa thông tin không phù hợp.";
        public static string AccountExists = "Email đã được sử dụng";
        public static string ErrorLogin = "Email hoặc mật khẩu không chính xác !";
    }
}
