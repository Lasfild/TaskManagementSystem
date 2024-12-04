using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagementSystem.BusinessLogic.Interfaces;
using TaskManagementSystem.DataAccess.Entities;
using TaskManagementSystem.DataAccess.Repositories.Interfaces;

namespace TaskManagementSystem.BusinessLogic.Services
{
    // Сервисный слой для управления пользователями
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository; // Репозиторий для взаимодействия с пользователями
        private readonly IConfiguration _configuration; // Конфигурация для доступа к настройкам, таким как ключи JWT

        // Конструктор с внедрением зависимостей
        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        // Метод для регистрации нового пользователя
        public async Task<bool> RegisterAsync(string username, string email, string password)
        {
            // Проверяем, существует ли пользователь с таким email
            var existingUser = await _userRepository.GetUserByEmailAsync(email);
            if (existingUser != null)
            {
                return false; // Если пользователь существует, регистрация не выполняется
            }

            // Создаем нового пользователя
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = username,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password), // Хэшируем пароль
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Сохраняем пользователя через репозиторий
            await _userRepository.AddUserAsync(user);
            return true;
        }

        // Метод для входа пользователя
        public async Task<string> LoginAsync(string usernameOrEmail, string password)
        {
            // Ищем пользователя по email или username
            var user = await _userRepository.GetUserByEmailAsync(usernameOrEmail)
                       ?? await _userRepository.GetUserByUsernameAsync(usernameOrEmail);

            // Проверяем существование пользователя и корректность пароля
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return null; // Возвращаем null при некорректных данных
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            // Создаем дескриптор токена с необходимыми claim
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id.ToString()), // ID пользователя
                    new Claim(JwtRegisteredClaimNames.Email, user.Email), // Email пользователя
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), // Subject
                    new Claim("role", "user") // Роль пользователя
                }),
                Expires = DateTime.UtcNow.AddHours(1), // Время истечения токена
                Issuer = _configuration["Jwt:Issuer"], // Издатель токена
                Audience = _configuration["Jwt:Audience"], // Аудитория токена
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature) // Подпись токена
            };

            // Генерируем JWT токен
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token); // Возвращаем сгенерированный токен
        }
    }
}
