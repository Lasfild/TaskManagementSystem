using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.BusinessLogic.Interfaces;
using TaskManagementSystem.Presentation.DTO.UserDTO;
using TaskManagementSystem.Presentation.DTOs;

namespace TaskManagementSystem.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        /// <summary>
        /// Конструктор контроллера пользователей.
        /// </summary>
        /// <param name="userService">Сервис управления пользователями.</param>
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Регистрация нового пользователя.
        /// </summary>
        /// <param name="registerDto">Объект с данными для регистрации.</param>
        /// <returns>Результат операции регистрации.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDTO registerDto)
        {
            // Проверяем, валидны ли данные, переданные клиентом
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // Возвращаем ошибки валидации

            // Вызываем сервис для регистрации пользователя
            var result = await _userService.RegisterAsync(registerDto.Username, registerDto.Email, registerDto.Password);
            if (!result)
                return BadRequest("Пользователь с таким email уже существует"); // Сообщаем, что email уже занят

            return Ok("Пользователь успешно зарегистрирован"); // Успешный ответ
        }

        /// <summary>
        /// Авторизация пользователя.
        /// </summary>
        /// <param name="loginDto">Объект с данными для авторизации.</param>
        /// <returns>JWT токен или ошибка авторизации.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO loginDto)
        {
            // Проверяем, валидны ли данные для авторизации
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // Возвращаем ошибки валидации

            // Вызываем сервис для авторизации пользователя
            var token = await _userService.LoginAsync(loginDto.UsernameOrEmail, loginDto.Password);
            if (string.IsNullOrEmpty(token))
                return Unauthorized("Неверный логин или пароль"); // Сообщаем об ошибке авторизации

            return Ok(new { Token = token }); // Возвращаем сгенерированный JWT токен
        }
    }
}
