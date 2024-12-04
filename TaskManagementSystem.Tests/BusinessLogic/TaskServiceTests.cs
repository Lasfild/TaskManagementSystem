using Moq;
using TaskEntity = TaskManagementSystem.DataAccess.Entities.Task; // Псевдоним для Task
using TaskManagementSystem.BusinessLogic.Services;
using TaskManagementSystem.DataAccess.Repositories.Interfaces;
using Xunit;

namespace TaskManagementSystem.Tests.BusinessLogic
{
    /// <summary>
    /// Тесты для сервиса задач (TaskService).
    /// </summary>
    public class TaskServiceTests
    {
        private readonly Mock<ITaskRepository> _taskRepositoryMock;
        private readonly TaskService _taskService;

        /// <summary>
        /// Инициализация моков и сервиса.
        /// </summary>
        public TaskServiceTests()
        {
            _taskRepositoryMock = new Mock<ITaskRepository>();
            _taskService = new TaskService(_taskRepositoryMock.Object);
        }

        /// <summary>
        /// Проверка метода GetTasksAsync: должен возвращать список задач и общее количество.
        /// </summary>
        [Fact]
        public async System.Threading.Tasks.Task GetTasksAsync_ShouldReturnTasks()
        {
            // Arrange: Подготавливаем тестовые данные и задаем поведение моков.
            var userId = Guid.NewGuid();
            var tasks = new List<TaskEntity>
            {
                new TaskEntity { Id = Guid.NewGuid(), Title = "Task 1", UserId = userId },
                new TaskEntity { Id = Guid.NewGuid(), Title = "Task 2", UserId = userId }
            };

            // Настраиваем мок репозитория, чтобы он возвращал тестовые данные.
            _taskRepositoryMock.Setup(repo => repo.GetTasksAsync(userId, null, null, null, false, 1, 10))
                               .ReturnsAsync((tasks, tasks.Count));

            // Act: Вызываем метод сервиса.
            var (resultTasks, totalCount) = await _taskService.GetTasksAsync(userId, null, null, null, false, 1, 10);

            // Assert: Проверяем результат.
            Assert.Equal(2, totalCount); // Проверяем общее количество задач.
            Assert.Equal(tasks, resultTasks); // Проверяем, что задачи совпадают.
        }
    }
}
