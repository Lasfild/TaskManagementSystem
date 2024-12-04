using TaskManagementSystem.BusinessLogic.Interfaces;
using TaskManagementSystem.DataAccess.Repositories.Interfaces;
using TaskEntity = TaskManagementSystem.DataAccess.Entities.Task;

namespace TaskManagementSystem.BusinessLogic.Services
{
    // Сервисный слой для работы с задачами
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        // Конструктор с внедрением зависимости для репозитория задач
        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        // Метод для получения списка задач с фильтрацией, сортировкой и пагинацией
        public async Task<(IEnumerable<TaskEntity> Tasks, int TotalCount)> GetTasksAsync(
            Guid userId, // ID пользователя, которому принадлежат задачи
            string? status, // Статус задачи для фильтрации
            string? priority, // Приоритет задачи для фильтрации
            string? sortBy = null, // Поле для сортировки (например, DueDate)
            bool desc = false, // Направление сортировки (по убыванию или возрастанию)
            int pageNumber = 1, // Номер страницы для пагинации
            int pageSize = 10 // Размер страницы для пагинации
        )
        {
            // Вызов соответствующего метода репозитория
            return await _taskRepository.GetTasksAsync(userId, status, priority, sortBy, desc, pageNumber, pageSize);
        }

        // Метод для получения задачи по её ID и ID пользователя
        public async Task<TaskEntity?> GetTaskByIdAsync(Guid taskId, Guid userId)
        {
            // Вызов репозитория для получения задачи
            return await _taskRepository.GetTaskByIdAsync(taskId, userId);
        }

        // Метод для создания новой задачи
        public async Task<TaskEntity> CreateTaskAsync(TaskEntity task)
        {
            // Установка временных меток для задачи
            task.CreatedAt = DateTime.UtcNow;
            task.UpdatedAt = DateTime.UtcNow;

            // Вызов репозитория для создания задачи
            return await _taskRepository.CreateTaskAsync(task);
        }

        // Метод для обновления существующей задачи
        public async Task<bool> UpdateTaskAsync(TaskEntity task)
        {
            // Вызов репозитория для обновления задачи
            return await _taskRepository.UpdateTaskAsync(task);
        }

        // Метод для удаления задачи
        public async Task<bool> DeleteTaskAsync(Guid taskId, Guid userId)
        {
            // Вызов репозитория для удаления задачи
            return await _taskRepository.DeleteTaskAsync(taskId, userId);
        }
    }
}
