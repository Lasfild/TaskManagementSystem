using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Presentation.DTO.UserDTO
{
    /// <summary>
    /// DTO для регистрации нового пользователя.
    /// </summary>
    public class RegisterUserDTO
    {
        /// <summary>
        /// Имя пользователя. Должно содержать от 3 до 50 символов. Обязательное поле.
        /// </summary>
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; }

        /// <summary>
        /// Email пользователя. Должен быть валидным адресом электронной почты. Обязательное поле.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Пароль пользователя. Должен содержать:
        /// минимум 8 символов, одну заглавную букву, одну строчную букву, одну цифру и один специальный символ.
        /// </summary>
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "Пароль должен содержать как минимум 8 символов, одну заглавную букву, одну строчную букву, одну цифру и один специальный символ.")]
        public string Password { get; set; }
    }
}
