using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Presentation.DTOs.TaskDTO
{
    /// <summary>
    /// DTO для создания новой задачи.
    /// </summary>
    public class CreateTaskDTO
    {
        /// <summary>
        /// Название задачи. Обязательно для заполнения.
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// Описание задачи. Может быть пустым.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Крайний срок выполнения задачи.
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Статус задачи. Принимает значения от 0 до 2, соответствующие TaskStatus.
        /// </summary>
        [Range(0, 2)]
        public int Status { get; set; }

        /// <summary>
        /// Приоритет задачи. Принимает значения от 0 до 2, соответствующие TaskPriority.
        /// </summary>
        [Range(0, 2)]
        public int Priority { get; set; }
    }
}
