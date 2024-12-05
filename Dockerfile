# Используем SDK образ для сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем решение и проекты
COPY TaskManagementSystem.sln .
COPY TaskManagementSystem.Presentation/TaskManagementSystem.Presentation.csproj TaskManagementSystem.Presentation/
COPY TaskManagementSystem.BusinessLogic/TaskManagementSystem.BusinessLogic.csproj TaskManagementSystem.BusinessLogic/
COPY TaskManagementSystem.DataAccess/TaskManagementSystem.DataAccess.csproj TaskManagementSystem.DataAccess/
COPY TaskManagementSystem.Tests/TaskManagementSystem.Tests.csproj TaskManagementSystem.Tests/

# Восстанавливаем зависимости
RUN dotnet restore

# Копируем оставшиеся файлы и собираем проект
COPY . .
WORKDIR /src/TaskManagementSystem.Presentation
RUN dotnet build -c Release -o /app/build

# Публикуем приложение
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Используем runtime-образ
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaskManagementSystem.Presentation.dll"]
