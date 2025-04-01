# ✈️ Ava.Platform

**Ava.Platform** is a unified, modular travel management system built for corporations that need streamlined control over employee travel policies, approval workflows, and booking rules — all in one place.

## 🧩 Project Structure

This monorepo brings together multiple projects under one cohesive solution:

```
Ava.Platform/
  ├── Ava.Shared         # Shared logic, models, DTOs, extensions, and utilities
  ├── Ava.API            # ASP.NET Core Web API for backend services and integrations
  ├── Ava.WebApp         # Blazor Server frontend for admin and end-user interaction
  ├── Ava.Deploy         # Infrastructure and deployment scripts (Docker, SQL, etc.)
  ├── compose.yaml       # Docker Compose configuration for the full stack
  ├── Ava.Platform.sln   # Root solution file tying all projects together
  ├── Dockerfile.API     # Dockerfile for building the API container
  ├── Dockerfile.WebApp  # Dockerfile for building the WebApp container
  ├── Dockerfile.builder # Dockerfile for building SDK container
  ├── CHANGELOG.md       # Project changelog
  ├── LICENSE            # Project LICENSE
  └── README.md          # Project documentation
```

Each project is loosely coupled and supports CI/CD, modular deployments, and separation of concerns.

---

## 🚀 Key Features

- ✅ Identity and access management (ASP.NET Identity)
- ✅ Corporate travel policy enforcement
- ✅ Smart booking restrictions and preferences
- ✅ Admin-first web interface via Blazor
- ✅ Open API backend for integrations
- ✅ Modular infrastructure (Docker-based)
- ✅ Health checks and self-healing via container orchestration

---

## 🧱 Tech Stack

| Layer        | Tech                       |
|--------------|----------------------------|
| Frontend     | Blazor Server (.NET 8+)    |
| Backend      | ASP.NET Core Web API       |
| Shared Code  | .NET Class Library         |
| Database     | PostgreSQL                 |
| Deployment   | Docker + Compose           |
| Infra Code   | Bash, SQL, PowerShell      |

---

## 📦 Getting Started

### 1. Clone the Repo

```bash
git clone https://github.com/your-org/Ava.Platform.git
cd Ava.Platform
```

### 2. Build and Run via Docker

```bash
docker compose up -d --build
```

### 3. Access the App

- **API:** http://localhost:5165/health
- **WebApp:** http://localhost:5090

## 🛠 Developer Guide

### Add a Migration

```bash
cd Ava.API
dotnet ef migrations add AddNewFeature -c AvaDbContext
```

### Run in Dev Mode

```bash
dotnet run --project Ava.WebApp
```

### Run Tests

```bash
dotnet test
```

## 🌐 Environment Configuration
Environment variables are managed per container or `.env` file (optional). You can configure DB connection strings, API keys, and custom settings there.


##📁 Notes

- All shared models, enums, and utility functions are located in `Ava.Shared`.
- Database triggers and initialization SQL scripts live in `Ava.Deploy/Resources/SQL`.
- Blazor and WebAPI rely on the shared library via project references.

## 👥 Contributors

- **You** — The visionary 🧠

## 📄 License

MIT — See [LICENSE](LICENSE) for details.
