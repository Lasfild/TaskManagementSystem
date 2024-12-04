namespace TaskManagementSystem.Presentation.DTOs.UserDTO
{
    /// <summary>
    /// DTO для представления данных пользователя.
    /// </summary>
    public class UserDTO
    {
        /// <summary>
        /// Уникальный идентификатор пользователя.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Email пользователя.
        /// </summary>
        public string Email { get; set; }
    }
}
