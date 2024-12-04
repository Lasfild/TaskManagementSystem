namespace TaskManagementSystem.Presentation.DTOs.TaskDTO
{
    /// <summary>
    /// DTO для отображения данных задачи.
    /// </summary>
    public class TaskDTO
    {
        /// <summary>
        /// Уникальный идентификатор задачи.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Название задачи.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Описание задачи.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Крайний срок выполнения задачи.
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Статус задачи. Представлен в виде числового значения (Enum TaskStatus).
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Приоритет задачи. Представлен в виде числового значения (Enum TaskPriority).
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Дата создания задачи.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Дата последнего обновления задачи.
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}
