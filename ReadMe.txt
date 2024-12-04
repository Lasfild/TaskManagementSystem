# Task Management System

## Project Overview
Task Management System is a RESTful web application designed for managing user tasks. Key features include:
- User authentication and registration with JWT-based security.
- CRUD operations for tasks with user-specific data scoping.
- Pagination, sorting, and filtering for task listings.
- Fully documented API using Swagger.

---

## Setup Instructions

### Prerequisites
- **.NET SDK 7.0 or later**
- **SQL Server** (local or remote)
- **Visual Studio 2022** or later with the following workloads:
  - ASP.NET and web development
  - .NET desktop development
- **Postman** (optional for API testing)

### Steps to Run Locally in Visual Studio

1. **Clone the Repository**:
   Open a terminal and run:
   ```bash
   git clone <repository-url>
   cd TaskManagementSystem

2. **Open the Project in Visual Studio**:
   Open Visual Studio.
   Select Open a Project or Solution.
   Navigate to the cloned repository folder and select the .sln file.

3. **Database is already configured for localhost**

4. **Set Up the Database: Open the Package Manager Console in Visual Studio**:
   Go to Tools > NuGet Package Manager > Package Manager Console.
   If there are no existing migrations in TaskManagementSystem.DataAccess > Migrations, run the following commands:

   Add-Migration InitialCreate
   Update-Database

   If migrations already exist, simply run:

   Update-Database

 5. **Run the Application**:
    Press F5 or click the Start button in Visual Studio.
    The application will start, and Swagger UI will be available at:
    https://localhost:7172/swagger

## API Documentation

### Authentication Endpoints

 1. **Register**
    Method: POST
    URL: /api/users/register
    Request Body:
     {
    "username": "string",
    "email": "string",
    "password": "string"
     }
    Response:
     200 OK: Successful registration.
     400 Bad Request: Invalid input or user already exists.

 2. **Login**
    Method: POST
    URL: /api/users/login
    Request Body:
     {
       "usernameOrEmail": "string",
       "password": "string"
     }
    Response:
     200 OK: Returns JWT token.
     401 Unauthorized: Invalid credentials.

### Task Endpoints
 1. **Get All Tasks**
    Method: GET
    URL: /api/tasks
    Query Parameters:
    status (optional): Filter by task status (e.g., Pending, InProgress, Completed).
    priority (optional): Filter by task priority (e.g., Low, Medium, High).
    sortBy (optional): Field to sort by (e.g., DueDate, Priority).
    desc (optional): Boolean to specify descending order.
    pageNumber (optional): Page number (default is 1).
    pageSize (optional): Number of tasks per page (default is 10).

 2. **Create Task**
    Method: POST
    URL: /api/tasks
    Request Body:
     {
       "title": "string",
       "description": "string",
       "dueDate": "2024-12-31",
       "status": 0,
       "priority": 1
     }
    Response:
     201 Created: Returns the created task.
     400 Bad Request: Validation error.

 3. **Update Task**
    Method: PUT
    URL: /api/tasks/{id}
    Request Body:
     {
      "title": "string",
       "description": "string",
       "dueDate": "2024-12-31",
       "status": 1,
       "priority": 2
     }
    Response:
     200 OK: Returns the updated task.
     404 Not Found: Task not found.
     
 4. **Delete Task**
    Method: DELETE
    URL: /api/tasks/{id}
    Response:
     204 No Content: Task deleted successfully.
     404 Not Found: Task not found.

## Architecture & Design Choices

### Layers
 1. **Presentation Layer**:
    Contains API controllers for handling HTTP requests and responses.
    Implements model validation and passes data to/from the service layer.

 2. **Business Logic Layer**:
    Contains service classes implementing business logic.
    Validates business rules and delegates data access to the repository layer.

 3. **Data Access Layer**:
    Contains Entity Framework Core repositories for interacting with the database.
    Defines the DbContext for managing entities and relationships.

### Dependency Injection
    All services and repositories are registered in Program.cs using built-in DI.
    Promotes testability and loose coupling.

### Authentication
    Uses JWT for secure authentication.
    Claims-based authorization for user-specific data scoping.

### Pagination, Sorting, and Filtering
    Implemented in the TaskRepository to optimize task queries.
    Supports dynamic sorting, filtering, and paginated responses.
