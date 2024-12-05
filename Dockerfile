# ���������� SDK ����� ��� ������
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# �������� ������� � �������
COPY TaskManagementSystem.sln .
COPY TaskManagementSystem.Presentation/TaskManagementSystem.Presentation.csproj TaskManagementSystem.Presentation/
COPY TaskManagementSystem.BusinessLogic/TaskManagementSystem.BusinessLogic.csproj TaskManagementSystem.BusinessLogic/
COPY TaskManagementSystem.DataAccess/TaskManagementSystem.DataAccess.csproj TaskManagementSystem.DataAccess/
COPY TaskManagementSystem.Tests/TaskManagementSystem.Tests.csproj TaskManagementSystem.Tests/

# ��������������� �����������
RUN dotnet restore

# �������� ���������� ����� � �������� ������
COPY . .
WORKDIR /src/TaskManagementSystem.Presentation
RUN dotnet build -c Release -o /app/build

# ��������� ����������
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# ���������� runtime-�����
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaskManagementSystem.Presentation.dll"]
