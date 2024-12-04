using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskEntity = TaskManagementSystem.DataAccess.Entities.Task; // Указываем псевдоним для Task
using TaskStatusEntity = TaskManagementSystem.DataAccess.Entities.TaskStatus; // Указываем псевдоним для TaskStatus
using TaskManagementSystem.DataAccess.Context;
using TaskManagementSystem.DataAccess.Repositories.Interfaces;

namespace TaskManagementSystem.DataAccess.Repositories
{
    /// <summary>
    /// Реализация репозитория для работы с задачами.
    /// </summary>
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskManagementDbContext _context;

        /// <summary>
        /// Конструктор TaskRepository.
        /// </summary>
        /// <param name="context">Контекст базы данных.</param>
        public TaskRepository(TaskManagementDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Получить список задач пользователя с фильтрацией, сортировкой и пагинацией.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="status">Статус задачи (опционально).</param>
        /// <param name="priority">Приоритет задачи (опционально).</param>
        /// <param name="sortBy">Поле для сортировки (опционально).</param>
        /// <param name="desc">Флаг сортировки по убыванию (по умолчанию false).</param>
        /// <param name="pageNumber">Номер страницы для пагинации (по умолчанию 1).</param>
        /// <param name="pageSize">Размер страницы для пагинации (по умолчанию 10).</param>
        /// <returns>Список задач и общее количество задач.</returns>
        public async Task<(IEnumerable<TaskEntity> Tasks, int TotalCount)> GetTasksAsync(
            Guid userId,
            string? status,
            string? priority,
            string? sortBy,
            bool desc,
            int pageNumber,
            int pageSize)
        {
            var query = _context.Tasks.AsQueryable();

            // Фильтрация по пользователю
            query = query.Where(t => t.UserId == userId);

            // Фильтрация по статусу
            if (!string.IsNullOrEmpty(status) && Enum.TryParse<TaskStatusEntity>(status, true, out var parsedStatus))
            {
                query = query.Where(t => t.Status == parsedStatus);
            }

            // Фильтрация по приоритету
            if (!string.IsNullOrEmpty(priority) && Enum.TryParse<TaskManagementSystem.DataAccess.Entities.TaskPriority>(priority, true, out var parsedPriority))
            {
                query = query.Where(t => t.Priority == parsedPriority);
            }

            // Сортировка
            query = sortBy switch
            {
                "DueDate" => desc ? query.OrderByDescending(t => t.DueDate) : query.OrderBy(t => t.DueDate),
                "Priority" => desc ? query.OrderByDescending(t => t.Priority) : query.OrderBy(t => t.Priority),
                _ => query.OrderBy(t => t.CreatedAt) // По умолчанию сортировка по CreatedAt
            };

            // Общее количество задач
            var totalCount = await query.CountAsync();

            // Пагинация
            var tasks = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (tasks, totalCount);
        }

        /// <summary>
        /// Получить задачу по идентификатору и идентификатору пользователя.
        /// </summary>
        /// <param name="taskId">Идентификатор задачи.</param>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <returns>Задача, если найдена; иначе null.</returns>
        public async Task<TaskEntity?> GetTaskByIdAsync(Guid taskId, Guid userId)
        {
            return await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId);
        }

        /// <summary>
        /// Создать новую задачу.
        /// </summary>
        /// <param name="task">Экземпляр задачи для добавления.</param>
        /// <returns>Добавленная задача.</returns>
        public async Task<TaskEntity> CreateTaskAsync(TaskEntity task)
        {
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();
            return task;
        }

        /// <summary>
        /// Обновить существующую задачу.
        /// </summary>
        /// <param name="task">Экземпляр задачи с обновленными данными.</param>
        /// <returns>Флаг успешности обновления.</returns>
        public async Task<bool> UpdateTaskAsync(TaskEntity task)
        {
            var existingTask = await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == task.Id && t.UserId == task.UserId);

            if (existingTask == null)
                return false;

            // Обновление данных задачи
            existingTask.Title = task.Title;
            existingTask.Description = task.Description;
            existingTask.DueDate = task.DueDate;
            existingTask.Status = task.Status;
            existingTask.Priority = task.Priority;
            existingTask.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Удалить задачу по идентификатору и идентификатору пользователя.
        /// </summary>
        /// <param name="taskId">Идентификатор задачи.</param>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <returns>Флаг успешности удаления.</returns>
        public async Task<bool> DeleteTaskAsync(Guid taskId, Guid userId)
        {
            var task = await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId);

            if (task == null)
                return false;

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
