# JobTrack

A full-stack job tracking application built with Angular and ASP.NET Core. Supports role-based access control for Admins and Users to manage job listings and applications.

**Live Demo:** [https://engprac-fullstack-fe.vercel.app/]
**API:** [https://engprac-fullstack-be.onrender.com/swagger]

---

## Features

- **Authentication** — JWT-based login and registration with BCrypt password hashing
- **Role-based Access** — Admin and User roles with different permissions
- **Job Management** — Admins can create, edit, and delete job listings
- **Job Applications** — Users can apply to open jobs and track their application status
- **Admin Controls** — Admins can view all applications and update status (Pass / Fail)
- **Dashboard** — Overview stats for total jobs, users, and applications

---

## Tech Stack

### Frontend (`engprac-fullstack-fe`)

| | |
|---|---|
| Framework | Angular 21 (Standalone Components, Zoneless) |
| UI Library | PrimeNG 21 with Aura theme |
| State Management | Angular Signals |
| HTTP | HttpClient with JWT interceptor |
| Routing | Angular Router with auth guards |
| Forms | Reactive Forms with validation |

### Backend (`engprac-fullstack-be`)

| | |
|---|---|
| Framework | ASP.NET Core 10 Web API |
| ORM | Entity Framework Core 10 |
| Database | PostgreSQL (Docker local / Neon production) |
| Authentication | JWT Bearer tokens |
| Password Hashing | BCrypt.Net |
| Documentation | Swagger / OpenAPI |
| Pattern | Repository + Service pattern |

---

## Project Structure

```
engprac-fullstack-be/          # Backend
├── Controllers/               # API endpoints
├── Data/                      # EF Core DbContext
├── DTOs/                      # Request & response models
├── Infrastructure/            # RoleAuthorize attribute
├── Migrations/                # EF Core migrations
├── Models/                    # Domain entities
├── Repositories/              # Data access layer
├── Services/                  # Business logic layer
└── Program.cs                 # App configuration

engprac-fullstack-fe/          # Frontend
└── src/app/
    ├── core/
    │   ├── guards/            # Auth & guest route guards
    │   ├── interceptors/      # JWT Bearer interceptor
    │   ├── models/            # TypeScript interfaces
    │   └── services/          # API service layer
    ├── layout/                # App shell with sidebar
    └── pages/
        ├── auth/              # Login & register
        ├── dashboard/         # Stats overview
        ├── jobs/              # Job listing & management
        └── applications/      # Application tracking
```

---

## API Endpoints

| Method | Endpoint | Role | Description |
|--------|----------|------|-------------|
| POST | `/api/v1/auth/register` | Public | Register new account |
| POST | `/api/v1/auth/login` | Public | Login and receive JWT |
| GET | `/api/v1/jobs` | Any | List all jobs |
| GET | `/api/v1/jobs/{id}` | Any | Get job by ID |
| POST | `/api/v1/jobs` | Admin | Create job |
| PUT | `/api/v1/jobs/{id}` | Admin | Update job |
| DELETE | `/api/v1/jobs/{id}` | Admin | Delete job |
| POST | `/api/v1/applications` | User | Apply to a job |
| GET | `/api/v1/applications` | Admin | Get all applications |
| GET | `/api/v1/applications/my` | User | Get my applications |
| DELETE | `/api/v1/applications/my/{id}` | User | Cancel application |
| PATCH | `/api/v1/applications/{id}/status` | Admin | Update application status |
| GET | `/api/v1/users` | Any | List all users |
| GET | `/api/v1/dashboard/summary` | Any | Get stats summary |

---

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js 20+](https://nodejs.org/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)

### Backend

```bash
cd engprac-fullstack-be

# Start PostgreSQL
docker compose up -d

# Run the API
dotnet run
```

API runs at `http://localhost:5118`  
Swagger UI at `http://localhost:5118/swagger`

### Frontend

```bash
cd engprac-fullstack-fe

npm install
npm start
```

App runs at `http://localhost:4200`

### Demo Accounts

| Email | Password | Role |
|-------|----------|------|
| alice@example.com | password123 | Admin |
| bob@example.com | password123 | User |
| carol@example.com | password123 | User |

---

## Deployment

| Service | Purpose |
|---------|---------|
| [Vercel](https://vercel.com) | Frontend hosting |
| [Render](https://render.com) | Backend hosting |
| [Neon](https://neon.tech) | PostgreSQL database |

### Environment Variables (Render)

| Key | Value |
|-----|-------|
| `ConnectionStrings__DefaultConnection` | Neon PostgreSQL connection string |
| `JwtSettings__Secret` | Random secret key (32+ characters) |
| `AllowedOrigins` | Vercel frontend URL |
| `ASPNETCORE_ENVIRONMENT` | `Production` |

### Vercel Build Settings

| Setting | Value |
|---------|-------|
| Framework Preset | Angular |
| Build Command | `npm run build` |
| Output Directory | `dist/frontend/browser` |
