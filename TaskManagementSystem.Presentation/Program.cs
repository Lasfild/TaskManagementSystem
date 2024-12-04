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

// ���������� ������������
builder.Services.AddControllers();

// ���������� ������� ��� API-������������ � �������������� Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // ��������� ������ �� ��-nullable ����
    options.SupportNonNullableReferenceTypes();
    options.EnableAnnotations();
    options.UseAllOfToExtendReferenceSchemas();

    // ��������� JWT �������������� ��� Swagger
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "������� ����� � ������� 'Bearer {�����}'",
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // ���������� �������������� � Swagger
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

// ����������� DbContext ��� ������ � ����� ������
builder.Services.AddDbContext<TaskManagementDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ��������� �������������� � �������������� JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, // �������� �������� ������
            ValidateAudience = true, // �������� ��������� ������
            ValidateLifetime = true, // �������� ������� ����� ������
            ValidateIssuerSigningKey = true, // �������� ����� ������� ������
            ValidIssuer = builder.Configuration["Jwt:Issuer"], // �������� ��������
            ValidAudience = builder.Configuration["Jwt:Audience"], // �������� ���������
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])) // ���� ��� �������
        };
    });

builder.Services.AddAuthorization(); // ���������� ��������� �����������

// ����������� ������������ (������ Repository)
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();

// ����������� �������� (������ Service)
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITaskService, TaskService>();

var app = builder.Build();

// ������������ ��������� ��������� HTTP-��������
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // ��������� Swagger � ������ ����������
    app.UseSwaggerUI(); // ��������� ����������������� ���������� Swagger
}

app.UseHttpsRedirection(); // ��������������� HTTP-�������� �� HTTPS

// ������������� �������������� �� ��� �������������� � �����������
app.UseAuthentication();
app.UseAuthorization();

// ������������� ������������
app.MapControllers();

// ������ ����������
app.Run();
