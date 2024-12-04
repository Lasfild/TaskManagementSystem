using System;
using System.Collections.Generic;
using TaskManagementSystem.DataAccess.Entities; // Директива для модели Task

namespace TaskManagementSystem.DataAccess.Entities
{
    // Модель данных для сущности пользователя
    public class User
    {
        public Guid Id { get; set; } // Уникальный идентификатор пользователя (первичный ключ)

        public string Username { get; set; } // Имя пользователя

        public string Email { get; set; } // Электронная почта пользователя (уникальная)

        public string PasswordHash { get; set; } // Хэшированный пароль пользователя

        public DateTime CreatedAt { get; set; } // Дата и время создания пользователя

        public DateTime UpdatedAt { get; set; } // Дата и время последнего обновления записи пользователя

        // Навигационное свойство для связанных задач
        public ICollection<Task> Tasks { get; set; } // Список задач, связанных с пользователем
    }
}
