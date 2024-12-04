using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.BusinessLogic.Interfaces;
using TaskManagementSystem.BusinessLogic.Services;
using TaskManagementSystem.DataAccess.Context;
using TaskManagementSystem.DataAccess.Repositories;
using TaskManagementSystem.DataAccess.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Добавление контроллеров
builder.Services.AddControllers();

// Добавление средств для API-документации с использованием Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Поддержка ссылок на не-nullable типы
    options.SupportNonNullableReferenceTypes();
    options.EnableAnnotations();
    options.UseAllOfToExtendReferenceSchemas();

    // Настройка JWT аутентификации для Swagger
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Введите токен в формате 'Bearer {токен}'",
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // Требование аутентификации в Swagger
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Регистрация DbContext для работы с базой данных
builder.Services.AddDbContext<TaskManagementDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Настройка аутентификации с использованием JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, // Проверка издателя токена
            ValidateAudience = true, // Проверка аудитории токена
            ValidateLifetime = true, // Проверка времени жизни токена
            ValidateIssuerSigningKey = true, // Проверка ключа подписи токена
            ValidIssuer = builder.Configuration["Jwt:Issuer"], // Валидный издатель
            ValidAudience = builder.Configuration["Jwt:Audience"], // Валидная аудитория
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])) // Ключ для подписи
        };
    });

builder.Services.AddAuthorization(); // Добавление поддержки авторизации

// Регистрация репозиториев (шаблон Repository)
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();

// Регистрация сервисов (шаблон Service)
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITaskService, TaskService>();

var app = builder.Build();

// Конфигурация конвейера обработки HTTP-запросов
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Включение Swagger в режиме разработки
    app.UseSwaggerUI(); // Включение пользовательского интерфейса Swagger
}

app.UseHttpsRedirection(); // Перенаправление HTTP-запросов на HTTPS

// Использование промежуточного ПО для аутентификации и авторизации
app.UseAuthentication();
app.UseAuthorization();

// Маршрутизация контроллеров
app.MapControllers();

// Запуск приложения
app.Run();
