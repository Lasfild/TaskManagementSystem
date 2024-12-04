using TaskEntity = TaskManagementSystem.DataAccess.Entities.Task; // Псевдоним для Task

namespace TaskManagementSystem.DataAccess.Repositories.Interfaces
{
    /// <summary>
    /// Интерфейс репозитория для работы с задачами.
    /// Содержит методы для CRUD-операций и получения задач с учетом фильтрации, сортировки и пагинации.
    /// </summary>
    public interface ITaskRepository
    {
        /// <summary>
        /// Получить список задач с учетом фильтрации, сортировки и пагинации.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя, чьи задачи нужно получить.</param>
        /// <param name="status">Фильтр по статусу задачи (опционально).</param>
        /// <param name="priority">Фильтр по приоритету задачи (опционально).</param>
        /// <param name="sortBy">Поле для сортировки (опционально).</param>
        /// <param name="desc">Сортировка по убыванию (true) или возрастанию (false).</param>
        /// <param name="pageNumber">Номер страницы для пагинации.</param>
        /// <param name="pageSize">Размер страницы для пагинации.</param>
        /// <returns>Кортеж, содержащий список задач и общее количество задач.</returns>
        Task<(IEnumerable<TaskEntity> Tasks, int TotalCount)> GetTasksAsync(
            Guid userId,
            string? status,
            string? priority,
            string? sortBy,
            bool desc,
            int pageNumber,
            int pageSize);

        /// <summary>
        /// Получить задачу по ее идентификатору и идентификатору пользователя.
        /// </summary>
        /// <param name="taskId">Идентификатор задачи.</param>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <returns>Задача, если она найдена; иначе null.</returns>
        Task<TaskEntity?> GetTaskByIdAsync(Guid taskId, Guid userId);

        /// <summary>
        /// Создать новую задачу.
        /// </summary>
        /// <param name="task">Экземпляр задачи для создания.</param>
        /// <returns>Созданная задача.</returns>
        Task<TaskEntity> CreateTaskAsync(TaskEntity task);

        /// <summary>
        /// Обновить существующую задачу.
        /// </summary>
        /// <param name="task">Экземпляр задачи с обновленными данными.</param>
        /// <returns>True, если задача успешно обновлена; иначе false.</returns>
        Task<bool> UpdateTaskAsync(TaskEntity task);

        /// <summary>
        /// Удалить задачу по идентификатору.
        /// </summary>
        /// <param name="taskId">Идентификатор задачи.</param>
        /// <param name="userId">Идентификатор пользователя, которому принадлежит задача.</param>
        /// <returns>True, если задача успешно удалена; иначе false.</returns>
        Task<bool> DeleteTaskAsync(Guid taskId, Guid userId);
    }
}
