namespace TaskManagementSystem.Presentation.DTOs.TaskDTO
{
    /// <summary>
    /// DTO для обновления данных задачи.
    /// </summary>
    public class UpdateTaskDTO
    {
        /// <summary>
        /// Новое название задачи. Может быть пустым.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Новое описание задачи. Может быть пустым.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Новый крайний срок выполнения задачи.
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Новый статус задачи. Может быть пустым.
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// Новый приоритет задачи. Может быть пустым.
        /// </summary>
        public int? Priority { get; set; }
    }
}
