using Moq; // Подключение библиотеки Moq для создания моков
using TaskManagementSystem.BusinessLogic.Services; // Подключение класса UserService
using TaskManagementSystem.DataAccess.Entities; // Подключение сущности User
using TaskManagementSystem.DataAccess.Repositories.Interfaces; // Подключение интерфейса IUserRepository
using Xunit; // Подключение библиотеки Xunit для написания тестов

namespace TaskManagementSystem.Tests.BusinessLogic
{
    /// <summary>
    /// Тесты для сервиса пользователя (UserService).
    /// </summary>
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock; // Мок репозитория пользователей
        private readonly UserService _userService; // Экземпляр сервиса пользователя

        /// <summary>
        /// Конструктор для инициализации моков и сервиса.
        /// </summary>
        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>(); // Создание мока для IUserRepository
            _userService = new UserService(_userRepositoryMock.Object, null); // Инициализация UserService с моком репозитория
        }

        /// <summary>
        /// Тест проверяет, что пользователь успешно регистрируется, если email отсутствует в базе данных.
        /// </summary>
        [Fact]
        public async System.Threading.Tasks.Task RegisterAsync_ShouldReturnTrue_WhenUserIsRegistered()
        {
            // Arrange: Настройка данных для теста
            var user = new User
            {
                Id = Guid.NewGuid(), // Уникальный идентификатор пользователя
                Username = "testuser", // Имя пользователя
                Email = "test@example.com", // Email пользователя
                PasswordHash = "hashedpassword" // Хэшированный пароль
            };

            // Настройка мока: метод GetUserByEmailAsync вернет null, если пользователь с данным email не найден
            _userRepositoryMock.Setup(repo => repo.GetUserByEmailAsync(user.Email)).ReturnsAsync((User)null);

            // Act: Вызов метода регистрации
            await _userService.RegisterAsync(user.Username, user.Email, user.PasswordHash);

            // Assert: Проверка, что метод AddUserAsync был вызван ровно один раз
            _userRepositoryMock.Verify(repo => repo.AddUserAsync(It.IsAny<User>()), Times.Once);
        }
    }
}
