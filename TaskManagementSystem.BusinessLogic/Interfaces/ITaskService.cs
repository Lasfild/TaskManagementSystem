using TaskEntity = TaskManagementSystem.DataAccess.Entities.Task; // Используем псевдоним для сущности Task
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace TaskManagementSystem.BusinessLogic.Interfaces
{
    public interface ITaskService
    {
        /// <summary>
        /// Получить список задач с фильтрацией, сортировкой и пагинацией.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="status">Фильтр по статусу.</param>
        /// <param name="priority">Фильтр по приоритету.</param>
        /// <param name="sortBy">Поле для сортировки (например, DueDate, Priority).</param>
        /// <param name="desc">Сортировка по убыванию (true/false).</param>
        /// <param name="pageNumber">Номер страницы.</param>
        /// <param name="pageSize">Размер страницы.</param>
        /// <returns>Список задач и общее количество.</returns>
        Task<(IEnumerable<TaskEntity> Tasks, int TotalCount)> GetTasksAsync(
            Guid userId,
            string? status,
            string? priority,
            string? sortBy = null,
            bool desc = false,
            int pageNumber = 1,
            int pageSize = 10);

        /// <summary>
        /// Получить задачу по идентификатору.
        /// </summary>
        /// <param name="taskId">Идентификатор задачи.</param>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <returns>Задача или null, если не найдена.</returns>
        Task<TaskEntity?> GetTaskByIdAsync(Guid taskId, Guid userId);

        /// <summary>
        /// Создать новую задачу.
        /// </summary>
        /// <param name="task">Данные задачи.</param>
        /// <returns>Созданная задача.</returns>
        Task<TaskEntity> CreateTaskAsync(TaskEntity task);

        /// <summary>
        /// Обновить существующую задачу.
        /// </summary>
        /// <param name="task">Обновленные данные задачи.</param>
        /// <returns>Успех операции (true/false).</returns>
        Task<bool> UpdateTaskAsync(TaskEntity task);

        /// <summary>
        /// Удалить задачу по идентификатору.
        /// </summary>
        /// <param name="taskId">Идентификатор задачи.</param>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <returns>Успех операции (true/false).</returns>
        Task<bool> DeleteTaskAsync(Guid taskId, Guid userId);
    }
}
