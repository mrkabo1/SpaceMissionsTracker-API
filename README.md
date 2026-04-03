# 🚀 Space Missions Tracker API

A RESTful API for tracking space missions, rockets, astronauts, and space agencies. Built with **.NET 8**, **Entity Framework Core**, and **JWT authentication**.

## 🌐 Live Demo
[Swagger UI](https://space-missions-tracker-api.runasp.net/swagger)

## ✨ Features
- JWT Authentication with Refresh Tokens
- Full CRUD operations for Agencies, Astronauts, Rockets, and Missions
- Many-to-many relationship (Mission ↔ Astronaut)
- Swagger/OpenAPI documentation
- ASP.NET Core Identity for user management
- Clean Architecture (Core, Infrastructure, WebApi layers)

## 🛠️ Tech Stack
- .NET 8
- Entity Framework Core
- SQL Server
- ASP.NET Core Identity
- JWT Bearer Authentication
- Swashbuckle (Swagger)
- MonsterASP.NET (hosting)

## 🏗️ Project Structure
- **SpaceMissionsTracker.Core** – Entities, DTOs, Service interfaces
- **SpaceMissionsTracker.Infrastructure** – DbContext, Migrations, Service implementations
- **SpaceMissionsTracker.WebApi** – Controllers, Program.cs, appsettings

## 🔐 API Endpoints (selected)
| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/Account/register` | Register new user |
| POST | `/api/Account/login` | Login → JWT token |
| POST | `/api/Account/get-new-token` | Refresh token |
| GET | `/api/Agencies` | Get all agencies |
| GET | `/api/Rockets` | Get all rockets (protected) |

*Full documentation available in Swagger UI.*

## 🧪 How to Run Locally
1. Clone the repository  
   `git clone https://github.com/YOUR_USERNAME/SpaceMissionsTracker-API.git`
2. Open the solution in Visual Studio 2022
3. Update the connection string in `appsettings.json` to point to your SQL Server
4. Run migrations: `Update-Database` in Package Manager Console
5. Run the project (F5)

## 📄 License
MIT License – free to use and modify.

## 👤 Author
Youssef Mohamed Gomaa – https://www.linkedin.com/in/youssef-mohamed-524b20214/
