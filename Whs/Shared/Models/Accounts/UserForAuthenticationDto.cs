using System;
using System.Collections.Generic;
using System.Text;

namespace Whs.Shared.Models.Accounts
{
    public class UserForAuthenticationDto
    {
        //[Required(ErrorMessage = "Email требуется.")]
        public string Email { get; set; }
        //[Required(ErrorMessage = "Пароль требуется.")]
        public string Password { get; set; }

        public string Barcode { get; set; }
    }

    public class AuthResponseDto
    {
        public bool IsAuthSuccessful { get; set; }
        public string ErrorMessage { get; set; }
        public string Token { get; set; }
    }
}
