using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserEntity = TaskManagementSystem.DataAccess.Entities.User; // Псевдоним для User
using TaskManagementSystem.DataAccess.Context;
using TaskManagementSystem.DataAccess.Repositories.Interfaces;

namespace TaskManagementSystem.DataAccess.Repositories
{
    /// <summary>
    /// Реализация репозитория для работы с пользователями.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly TaskManagementDbContext _context;

        /// <summary>
        /// Конструктор UserRepository.
        /// </summary>
        /// <param name="context">Контекст базы данных.</param>
        public UserRepository(TaskManagementDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получить пользователя по идентификатору.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <returns>Объект пользователя или null, если пользователь не найден.</returns>
        public async Task<UserEntity?> GetUserByIdAsync(Guid userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        /// <summary>
        /// Получить пользователя по email.
        /// </summary>
        /// <param name="email">Email пользователя.</param>
        /// <returns>Объект пользователя или null, если пользователь не найден.</returns>
        public async Task<UserEntity?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        /// <summary>
        /// Получить пользователя по имени пользователя (username).
        /// </summary>
        /// <param name="username">Имя пользователя.</param>
        /// <returns>Объект пользователя или null, если пользователь не найден.</returns>
        public async Task<UserEntity?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        /// <summary>
        /// Добавить нового пользователя.
        /// </summary>
        /// <param name="user">Объект пользователя для добавления.</param>
        public async Task AddUserAsync(UserEntity user)
        {
            await _context.Users.AddAsync(user); // Асинхронное добавление нового пользователя.
            await _context.SaveChangesAsync(); // Сохранение изменений в базе данных.
        }

        /// <summary>
        /// Обновить существующего пользователя.
        /// </summary>
        /// <param name="user">Объект пользователя с обновленными данными.</param>
        public async Task UpdateUserAsync(UserEntity user)
        {
            _context.Users.Update(user); // Обновление данных пользователя.
            await _context.SaveChangesAsync(); // Сохранение изменений в базе данных.
        }
    }
}
