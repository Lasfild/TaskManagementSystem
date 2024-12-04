using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Presentation.DTO.UserDTO
{
    /// <summary>
    /// DTO для входа пользователя в систему.
    /// </summary>
    public class LoginUserDTO
    {
        /// <summary>
        /// Имя пользователя или email. Обязательное поле.
        /// </summary>
        [Required]
        public string UsernameOrEmail { get; set; }

        /// <summary>
        /// Пароль пользователя. Обязательное поле.
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
