using Microsoft.EntityFrameworkCore;
using TaskEntity = TaskManagementSystem.DataAccess.Entities.Task; // Псевдоним для Task
using TaskManagementSystem.DataAccess.Entities;

namespace TaskManagementSystem.DataAccess.Context
{
    // Контекст базы данных для системы управления задачами
    public class TaskManagementDbContext : DbContext
    {
        // Конструктор для передачи параметров конфигурации
        public TaskManagementDbContext(DbContextOptions<TaskManagementDbContext> options) : base(options) { }

        // DbSet для пользователей
        public DbSet<User> Users { get; set; }
        // DbSet для задач
        public DbSet<TaskEntity> Tasks { get; set; }

        // Метод для конфигурации моделей
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Уникальный индекс на поле Email в таблице Users
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

            // Настройка связи "один ко многим" между User и TaskEntity
            modelBuilder.Entity<User>()
                .HasMany(u => u.Tasks) // У пользователя может быть много задач
                .WithOne(t => t.User) // Каждая задача связана с одним пользователем
                .HasForeignKey(t => t.UserId) // Внешний ключ для задачи - UserId
                .OnDelete(DeleteBehavior.Cascade); // Удаление пользователя приведет к удалению всех его задач

            // Вызов базового метода для завершения конфигурации
            base.OnModelCreating(modelBuilder);
        }
    }
}
