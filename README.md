# Encadri

Academic project supervision and management platform for coordinating student-supervisor interactions, project tracking, and document management.

## Overview

Encadri is a full-stack web application designed to streamline academic project supervision workflows. The platform facilitates communication between students and supervisors, manages project milestones, handles document submissions, and provides real-time collaboration features.

## Features

### Project Management
- Project creation and assignment
- Milestone tracking and deadline management
- Project status monitoring and reporting
- Task assignment and progress tracking

### Communication
- Real-time chat system with SignalR integration
- Private messaging between students and supervisors
- Real-time notifications for project updates
- Message history and conversation management

### Document Management
- Document upload and storage (Azure Blob Storage support)
- Multiple file format support
- Document versioning and submission tracking
- File preview and download capabilities

### Evaluation System
- Project evaluation and grading workflow
- Submission review and feedback mechanism
- Evaluation criteria management
- Historical evaluation tracking

### Meeting Coordination
- Meeting scheduling between students and supervisors
- Calendar integration
- Meeting status tracking (scheduled, completed, cancelled)
- Meeting notes and outcome documentation

### User Management
- Role-based access control (Student, Supervisor, Admin)
- User authentication and authorization
- Profile management
- User activity tracking

## Tech Stack

### Frontend
- **Framework**: Angular 18.2
- **Language**: TypeScript 5.5
- **Real-time**: SignalR Client 10.0
- **State Management**: RxJS 7.8
- **Build Tool**: Angular CLI 18.2

### Backend
- **Framework**: ASP.NET Core 8.0
- **Language**: C# 12.0
- **Database**: PostgreSQL / SQL Server / MySQL (EF Core 8.0)
- **Real-time**: SignalR 1.1
- **API Documentation**: Swagger/OpenAPI
- **Cloud Storage**: Azure Blob Storage 12.19
- **ORM**: Entity Framework Core 8.0

## Project Structure

```
encadri-V1/
├── Encadri-Frontend/
│   ├── src/
│   │   ├── app/
│   │   │   ├── core/              # Core services and guards
│   │   │   ├── features/          # Feature modules
│   │   │   │   ├── auth/          # Authentication
│   │   │   │   ├── projects/      # Project management
│   │   │   │   ├── chat/          # Real-time chat
│   │   │   │   ├── documents/     # Document repository
│   │   │   │   ├── meetings/      # Meeting scheduler
│   │   │   │   ├── evaluations/   # Evaluation system
│   │   │   │   ├── milestones/    # Milestone tracking
│   │   │   │   ├── submissions/   # Submission management
│   │   │   │   ├── dashboard/     # User dashboard
│   │   │   │   └── profile/       # User profile
│   │   │   ├── layout/            # Layout components
│   │   │   └── shared/            # Shared components
│   │   ├── environments/          # Environment configs
│   │   └── assets/                # Static assets
│   ├── angular.json
│   └── package.json
│
└── Encadri-Backend/
    ├── Encadri-Backend.sln
    └── Encadri-Backend/
        ├── Controllers/           # API controllers
        ├── Models/                # Domain models
        ├── Data/                  # Database context
        ├── Services/              # Business logic
        ├── Hubs/                  # SignalR hubs
        ├── Migrations/            # EF migrations
        ├── Helpers/               # Utility classes
        ├── Properties/            # Project properties
        ├── wwwroot/               # Static files
        ├── Program.cs             # Application entry point
        ├── appsettings.json       # Configuration
        └── Encadri-Backend.csproj
```

## Prerequisites

- **Node.js**: 18.x or higher
- **npm**: 9.x or higher
- **Angular CLI**: 18.2.21 or higher
- **.NET SDK**: 8.0 or higher
- **Database**: PostgreSQL 14+ / SQL Server 2019+ / MySQL 8.0+
- **Azure Storage Account**: (Optional, for document storage)

## Installation

### Frontend Setup

```bash
cd Encadri-Frontend
npm install
```

### Backend Setup

```bash
cd Encadri-Backend/Encadri-Backend
dotnet restore
```

## Configuration

### Frontend Environment

Update `Encadri-Frontend/src/environments/environment.ts`:

```typescript
export const environment = {
  production: false,
  apiUrl: 'https://localhost:7165/api',
  hubUrl: 'https://localhost:7165/hubs'
};
```

### Backend Configuration

Update `Encadri-Backend/Encadri-Backend/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=encadri;Username=postgres;Password=yourpassword"
  },
  "AzureStorage": {
    "ConnectionString": "your-azure-storage-connection-string",
    "ContainerName": "documents"
  }
}
```

**Supported Database Providers:**
- PostgreSQL: `Npgsql.EntityFrameworkCore.PostgreSQL`
- SQL Server: `Microsoft.EntityFrameworkCore.SqlServer`
- MySQL: `Pomelo.EntityFrameworkCore.MySql`

## Database Setup

Apply Entity Framework migrations:

```bash
cd Encadri-Backend/Encadri-Backend
dotnet ef database update
```

## Running the Application

### Start Frontend (Development)

```bash
cd Encadri-Frontend
ng serve
```

Application runs at: `http://localhost:4200`

### Start Backend (Development)

```bash
cd Encadri-Backend/Encadri-Backend
dotnet run
```

API runs at: `https://localhost:7165`
Swagger UI: `https://localhost:7165/swagger`

### Run Both Concurrently

Open two terminal windows and execute the commands above in parallel.

## Build for Production

### Frontend Production Build

```bash
cd Encadri-Frontend
ng build --configuration production
```

Build output: `Encadri-Frontend/dist/`

### Backend Production Build

```bash
cd Encadri-Backend/Encadri-Backend
dotnet publish -c Release -o ./publish
```

Build output: `Encadri-Backend/Encadri-Backend/publish/`

## API Endpoints

The backend exposes the following API controllers:

- `/api/auth` - Authentication and authorization
- `/api/projects` - Project management
- `/api/messages` - Chat and messaging
- `/api/documents` - Document repository
- `/api/meetings` - Meeting scheduling
- `/api/evaluations` - Evaluation system
- `/api/milestones` - Milestone tracking
- `/api/submissions` - Submission management
- `/api/notifications` - User notifications

**SignalR Hubs:**
- `/hubs/chat` - Real-time chat
- `/hubs/notification` - Real-time notifications

## Troubleshooting

### Frontend Issues

**CORS errors:**
- Verify `apiUrl` in environment configuration
- Check backend CORS policy in `Program.cs`

**SignalR connection failed:**
- Confirm `hubUrl` matches backend configuration
- Check firewall settings for WebSocket connections

### Backend Issues

**Database connection failed:**
- Verify connection string in `appsettings.json`
- Ensure database server is running
- Check network connectivity and credentials

**Migration errors:**
- Delete `Migrations` folder and regenerate: `dotnet ef migrations add InitialCreate`
- Verify database provider package is installed

**Azure Blob Storage errors:**
- Validate connection string format
- Confirm container exists and permissions are set
- Azure Storage is optional; file uploads will fail if not configured

## Development

### Code Generation

**Angular component:**
```bash
ng generate component features/feature-name/component-name
```

**Angular service:**
```bash
ng generate service core/services/service-name
```

### Database Migrations

**Add migration:**
```bash
dotnet ef migrations add MigrationName
```

**Remove last migration:**
```bash
dotnet ef migrations remove
```

## License

This project is intended for academic and educational purposes.
