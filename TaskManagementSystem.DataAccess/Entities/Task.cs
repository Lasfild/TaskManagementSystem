using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TaskManagementSystem.DataAccess.Entities
{
    // Модель данных для сущности задачи
    public class Task
    {
        [Key] // Указывает, что поле является первичным ключом
        public Guid Id { get; set; }

        [Required] // Указывает, что поле обязательно для заполнения
        public string Title { get; set; } // Название задачи

        public string? Description { get; set; } // Описание задачи (необязательно)

        public DateTime? DueDate { get; set; } // Дата выполнения задачи (необязательно)

        [Required] // Указывает, что поле обязательно для заполнения
        public TaskStatus Status { get; set; } // Статус задачи

        [Required] // Указывает, что поле обязательно для заполнения
        public TaskPriority Priority { get; set; } // Приоритет задачи

        // Поле для хранения даты создания задачи
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Поле для хранения даты последнего обновления задачи
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Required] // Указывает, что поле обязательно для заполнения
        public Guid UserId { get; set; } // Внешний ключ, связанный с пользователем

        [ForeignKey(nameof(UserId))] // Указывает, что UserId является внешним ключом
        [JsonIgnore] // Исключает поле из сериализации в JSON
        public User User { get; set; } // Навигационное свойство для связи с пользователем
    }

    // Перечисление статусов задачи
    public enum TaskStatus
    {
        Pending,      // В ожидании
        InProgress,   // В процессе выполнения
        Completed     // Выполнено
    }

    // Перечисление приоритетов задачи
    public enum TaskPriority
    {
        Low,          // Низкий приоритет
        Medium,       // Средний приоритет
        High          // Высокий приоритет
    }
}
