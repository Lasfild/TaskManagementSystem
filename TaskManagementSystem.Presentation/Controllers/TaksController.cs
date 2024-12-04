using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.BusinessLogic.Interfaces;
using TaskEntity = TaskManagementSystem.DataAccess.Entities.Task; // Псевдоним для Task
using TaskStatus = TaskManagementSystem.DataAccess.Entities.TaskStatus;
using TaskPriority = TaskManagementSystem.DataAccess.Entities.TaskPriority;
using TaskManagementSystem.DataAccess.Entities;
using TaskManagementSystem.Presentation.DTOs.TaskDTO;

namespace TaskManagementSystem.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Требует аутентификации для всех методов контроллера
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        /// <summary>
        /// Конструктор контроллера задач.
        /// </summary>
        /// <param name="taskService">Сервис для управления задачами.</param>
        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        /// <summary>
        /// Получить список задач текущего пользователя с возможностью фильтрации, сортировки и пагинации.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetTasks(
            [FromQuery] string? status, // Фильтрация по статусу
            [FromQuery] string? priority, // Фильтрация по приоритету
            [FromQuery] string? sortBy, // Поле для сортировки
            [FromQuery] bool desc = false, // Сортировка по убыванию
            [FromQuery] int pageNumber = 1, // Номер страницы
            [FromQuery] int pageSize = 10) // Размер страницы
        {
            var userId = Guid.Parse(User.FindFirst("Id").Value); // Извлекаем ID пользователя из токена
            var (tasks, totalCount) = await _taskService.GetTasksAsync(userId, status, priority, sortBy, desc, pageNumber, pageSize);

            // Формируем ответ с пагинацией
            var response = new
            {
                totalCount,
                pageNumber,
                pageSize,
                tasks
            };

            return Ok(response); // Возвращаем результат
        }

        /// <summary>
        /// Получить задачу по ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(Guid id)
        {
            var userId = Guid.Parse(User.FindFirst("Id").Value); // Получаем ID пользователя из токена
            var task = await _taskService.GetTaskByIdAsync(id, userId);

            if (task == null)
                return NotFound(); // Задача не найдена

            return Ok(task); // Возвращаем найденную задачу
        }

        /// <summary>
        /// Создать новую задачу.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskDTO createTaskDTO)
        {
            var userId = Guid.Parse(User.FindFirst("Id").Value); // ID текущего пользователя

            // Создаем задачу с преобразованием данных
            var task = new TaskEntity
            {
                Title = createTaskDTO.Title,
                Description = createTaskDTO.Description,
                DueDate = createTaskDTO.DueDate,
                Status = (TaskStatus)createTaskDTO.Status, // Приведение int -> TaskStatus
                Priority = (TaskPriority)createTaskDTO.Priority, // Приведение int -> TaskPriority
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var createdTask = await _taskService.CreateTaskAsync(task);
            return CreatedAtAction(nameof(GetTaskById), new { id = createdTask.Id }, createdTask); // Возвращаем созданную задачу
        }

        /// <summary>
        /// Обновить существующую задачу.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(Guid id, [FromBody] UpdateTaskDTO updateTaskDTO)
        {
            var userId = Guid.Parse(User.FindFirst("Id")?.Value); // ID текущего пользователя

            // Проверяем, принадлежит ли задача пользователю
            var existingTask = await _taskService.GetTaskByIdAsync(id, userId);
            if (existingTask == null)
            {
                return NotFound("Задача не найдена или вы не имеете к ней доступа.");
            }

            // Обновляем свойства задачи
            existingTask.Title = updateTaskDTO.Title ?? existingTask.Title;
            existingTask.Description = updateTaskDTO.Description ?? existingTask.Description;
            existingTask.DueDate = updateTaskDTO.DueDate ?? existingTask.DueDate;
            existingTask.Status = updateTaskDTO.Status.HasValue
                ? (TaskStatus)updateTaskDTO.Status.Value
                : existingTask.Status;
            existingTask.Priority = updateTaskDTO.Priority.HasValue
                ? (TaskPriority)updateTaskDTO.Priority.Value
                : existingTask.Priority;
            existingTask.UpdatedAt = DateTime.UtcNow;

            var result = await _taskService.UpdateTaskAsync(existingTask);

            if (!result)
            {
                return StatusCode(500, "Не удалось обновить задачу."); // Ошибка обновления
            }

            return Ok(existingTask); // Возвращаем обновленную задачу
        }

        /// <summary>
        /// Удалить задачу по ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            var userId = Guid.Parse(User.FindFirst("Id").Value); // ID текущего пользователя
            var result = await _taskService.DeleteTaskAsync(id, userId);

            if (!result)
                return NotFound(); // Задача не найдена

            return NoContent(); // Успешное удаление
        }
    }
}
