using Microsoft.AspNetCore.Mvc;
using Moq;
using TaskManagementSystem.BusinessLogic.Interfaces;
using TaskManagementSystem.Presentation.Controllers;
using TaskManagementSystem.Presentation.DTO.UserDTO;
using Xunit;

namespace TaskManagementSystem.Tests.Presentation
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _controller = new UserController(_userServiceMock.Object);
        }

        /// <summary>
        /// Проверка метода Login: должен возвращать токен при корректных данных для входа.
        /// </summary>
        [Fact]
        public async System.Threading.Tasks.Task Login_ShouldReturnToken_WhenCredentialsAreValid()
        {
            // Arrange: Подготавливаем тестовые данные.
            var loginDto = new LoginUserDTO
            {
                UsernameOrEmail = "testuser",
                Password = "Password@123"
            };

            var expectedToken = "fake-jwt-token";

            // Настраиваем мок сервиса.
            _userServiceMock
                .Setup(service => service.LoginAsync(loginDto.UsernameOrEmail, loginDto.Password))
                .ReturnsAsync(expectedToken);

            // Act: Вызываем метод контроллера.
            var result = await _controller.Login(loginDto);

            // Assert: Проверяем результат.
            var okResult = Assert.IsType<OkObjectResult>(result); // Ожидаем OkObjectResult.
            var response = okResult.Value; // Получаем значение ответа.

            // Проверяем свойство Token в анонимном типе.
            Assert.NotNull(response); // Убедимся, что ответ не пустой.
            Assert.Equal(expectedToken, response.GetType().GetProperty("Token")?.GetValue(response)); // Проверяем токен.
        }
    }
}
