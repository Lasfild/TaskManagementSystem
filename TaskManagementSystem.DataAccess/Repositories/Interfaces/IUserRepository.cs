using System;
using System.Threading.Tasks;
using UserEntity = TaskManagementSystem.DataAccess.Entities.User; // Псевдоним для User

namespace TaskManagementSystem.DataAccess.Repositories.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория для работы с пользователями.
    /// Содержит методы для выполнения операций CRUD с пользователями.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Получить пользователя по его идентификатору.
        /// </summary>
        /// <param name="userId">Уникальный идентификатор пользователя.</param>
        /// <returns>Пользователь, если найден; иначе null.</returns>
        Task<UserEntity?> GetUserByIdAsync(Guid userId);

        /// <summary>
        /// Получить пользователя по его email.
        /// </summary>
        /// <param name="email">Email пользователя.</param>
        /// <returns>Пользователь, если найден; иначе null.</returns>
        Task<UserEntity?> GetUserByEmailAsync(string email);

        /// <summary>
        /// Получить пользователя по его имени пользователя (username).
        /// </summary>
        /// <param name="username">Имя пользователя.</param>
        /// <returns>Пользователь, если найден; иначе null.</returns>
        Task<UserEntity?> GetUserByUsernameAsync(string username);

        /// <summary>
        /// Добавить нового пользователя в базу данных.
        /// </summary>
        /// <param name="user">Экземпляр пользователя для добавления.</param>
        /// <returns>Асинхронная задача.</returns>
        Task AddUserAsync(UserEntity user);

        /// <summary>
        /// Обновить данные существующего пользователя.
        /// </summary>
        /// <param name="user">Экземпляр пользователя с обновленными данными.</param>
        /// <returns>Асинхронная задача.</returns>
        Task UpdateUserAsync(UserEntity user);
    }
}
