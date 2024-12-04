using System; // Для Guid
using System.Threading.Tasks; // Для Task
using Microsoft.EntityFrameworkCore; // Для использования InMemoryDatabase
using TaskManagementSystem.DataAccess.Context; // Контекст базы данных
using TaskManagementSystem.DataAccess.Entities; // Сущность User
using TaskManagementSystem.DataAccess.Repositories; // Репозиторий UserRepository
using Xunit; // Библиотека тестирования Xunit

namespace TaskManagementSystem.Tests.DataAccess.Repositories
{
    public class UserRepositoryTests
    {
        private readonly TaskManagementDbContext _context; // Контекст базы данных
        private readonly UserRepository _repository; // Тестируемый репозиторий

        public UserRepositoryTests()
        {
            // Настройка базы данных в памяти для тестирования
            var options = new DbContextOptionsBuilder<TaskManagementDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new TaskManagementDbContext(options);
            _repository = new UserRepository(_context);
        }

        /// <summary>
        /// Проверяет, что пользователь добавляется в базу данных.
        /// </summary>
        [Fact]
        public async System.Threading.Tasks.Task AddUserAsync_ShouldAddUserToDatabase()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = "testuser",
                Email = "test@example.com",
                PasswordHash = "hashedpassword"
            };

            // Act
            await _repository.AddUserAsync(user);

            // Assert
            var result = await _context.Users.FindAsync(user.Id);
            Assert.NotNull(result); // Проверяем, что пользователь добавлен.
            Assert.Equal("testuser", result.Username); // Проверяем, что имя пользователя совпадает.
        }

        /// <summary>
        /// Проверяет, что пользователь возвращается, если email существует в базе данных.
        /// </summary>
        [Fact]
        public async System.Threading.Tasks.Task GetUserByEmailAsync_ShouldReturnUser_WhenEmailExists()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = "testuser",
                Email = "test@example.com",
                PasswordHash = "hashedpassword"
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserByEmailAsync("test@example.com");

            // Assert
            Assert.NotNull(result); // Проверяем, что пользователь найден.
            Assert.Equal(user.Id, result.Id); // Проверяем, что Id совпадает.
        }

        /// <summary>
        /// Проверяет, что данные пользователя обновляются в базе данных.
        /// </summary>
        [Fact]
        public async System.Threading.Tasks.Task UpdateUserAsync_ShouldUpdateUserDetails()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = "testuser",
                Email = "test@example.com",
                PasswordHash = "hashedpassword"
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Act
            user.Username = "updateduser";
            await _repository.UpdateUserAsync(user);

            // Assert
            var updatedUser = await _context.Users.FindAsync(user.Id);
            Assert.Equal("updateduser", updatedUser.Username); // Проверяем, что имя пользователя обновлено.
        }
    }
}
