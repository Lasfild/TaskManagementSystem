using Microsoft.AspNetCore.Mvc;
using Moq;
using TaskEntity = TaskManagementSystem.DataAccess.Entities.Task; // Псевдоним для Task
using TaskManagementSystem.BusinessLogic.Interfaces;
using TaskManagementSystem.Presentation.Controllers;
using Xunit;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace TaskManagementSystem.Tests.Controllers
{
    public class TaskControllerTests
    {
        private readonly Mock<ITaskService> _taskServiceMock;
        private readonly TaskController _controller;

        public TaskControllerTests()
        {
            _taskServiceMock = new Mock<ITaskService>();
            _controller = new TaskController(_taskServiceMock.Object);

            // Mocking HttpContext to simulate authenticated user.
            var userId = Guid.NewGuid().ToString();
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("Id", userId)
            }));
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        /// <summary>
        /// Проверка метода GetTasks: должен возвращать OK результат с задачами.
        /// </summary>
        [Fact]
        public async System.Threading.Tasks.Task GetTasks_ShouldReturnOkResult()
        {
            // Arrange: Подготавливаем тестовые данные.
            var userId = Guid.NewGuid();
            var tasks = new List<TaskEntity>
            {
                new TaskEntity { Id = Guid.NewGuid(), Title = "Task 1", UserId = userId },
                new TaskEntity { Id = Guid.NewGuid(), Title = "Task 2", UserId = userId }
            };

            // Настраиваем мок сервиса.
            _taskServiceMock
                .Setup(service => service.GetTasksAsync(userId, null, null, null, false, 1, 10))
                .ReturnsAsync((tasks, tasks.Count));

            // Устанавливаем контекст для контроллера.
            var controller = new TaskController(_taskServiceMock.Object);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("Id", userId.ToString())
            }));

            // Act: Вызываем метод контроллера.
            var result = await controller.GetTasks(null, null, null, false, 1, 10);

            // Assert: Проверяем результат.
            var okResult = Assert.IsType<OkObjectResult>(result); // Ожидаем OkObjectResult.
            var response = okResult.Value; // Получаем значение ответа.

            // Проверяем свойства в анонимном типе.
            Assert.NotNull(response); // Убедимся, что ответ не пустой.
            Assert.Equal(tasks.Count, response.GetType().GetProperty("totalCount")?.GetValue(response)); // Проверяем totalCount.
            Assert.Equal(tasks, response.GetType().GetProperty("tasks")?.GetValue(response)); // Проверяем список задач.
        }
    }
}
