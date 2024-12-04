using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskEntity = TaskManagementSystem.DataAccess.Entities.Task; // Указываем псевдоним для Task
using TaskStatusEntity = TaskManagementSystem.DataAccess.Entities.TaskStatus; // Указываем псевдоним для TaskStatus
using TaskPriorityEntity = TaskManagementSystem.DataAccess.Entities.TaskPriority; // Указываем псевдоним для TaskPriority
using TaskManagementSystem.DataAccess.Context;
using TaskManagementSystem.DataAccess.Repositories;
using Xunit;

namespace TaskManagementSystem.Tests.DataAccess.Repositories
{
    /// <summary>
    /// Тесты для репозитория задач (TaskRepository).
    /// </summary>
    public class TaskRepositoryTests
    {
        private readonly TaskManagementDbContext _context;
        private readonly TaskRepository _repository;

        /// <summary>
        /// Инициализация InMemoryDatabase и репозитория.
        /// </summary>
        public TaskRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<TaskManagementDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Используем InMemoryDatabase.
                .Options;

            _context = new TaskManagementDbContext(options);
            _repository = new TaskRepository(_context);
        }

        /// <summary>
        /// Проверка метода CreateTaskAsync: задача должна добавляться в базу данных.
        /// </summary>
        [Fact]
        public async Task CreateTaskAsync_ShouldAddTaskToDatabase()
        {
            // Arrange: Подготавливаем тестовые данные.
            var task = new TaskEntity
            {
                Id = Guid.NewGuid(),
                Title = "Test Task",
                UserId = Guid.NewGuid(),
                Status = TaskStatusEntity.Pending, // Используем псевдоним для TaskStatus.
                Priority = TaskPriorityEntity.Medium // Используем псевдоним для TaskPriority.
            };

            // Act: Вызываем метод репозитория.
            var createdTask = await _repository.CreateTaskAsync(task);

            // Assert: Проверяем, что задача добавлена в базу данных.
            var result = await _context.Tasks.FindAsync(task.Id);
            Assert.NotNull(result); // Убедимся, что задача существует.
            Assert.Equal("Test Task", result.Title); // Проверяем название задачи.
        }
    }
}
