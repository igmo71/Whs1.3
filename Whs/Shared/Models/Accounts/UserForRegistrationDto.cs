using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Whs.Shared.Models.Accounts
{
    public class UserForRegistrationDto
    {
        [Required(ErrorMessage = "Имя обязательно")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Электронная почта требуется")]
        public string Email { get; set; }

        //[Required(ErrorMessage = "Пароль обязателен")]
        public string Password { get; set; }

        //[Compare("Password", ErrorMessage = "Пароль и подтверждение не совпадают.")]
        public string ConfirmPassword { get; set; }

        //[Required(ErrorMessage = "Укажите склад.")]
        public string WarehouseId { get; set; }
    }

    public class RegistrationResponseDto
    {
        public bool IsSuccessfulRegistration { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
