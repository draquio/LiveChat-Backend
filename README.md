# LiveChat Backend

A real-time chat application backend built with **ASP.NET Core 9.0**, featuring WebSocket-based messaging via **SignalR**, JWT authentication, file uploads, and support for both authenticated and guest users.

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Tech Stack](#tech-stack)
- [Architecture](#architecture)
- [Project Structure](#project-structure)
- [API Reference](#api-reference)
- [SignalR Hub](#signalr-hub)
- [Database Schema](#database-schema)
- [Getting Started](#getting-started)
- [Docker](#docker)
- [Configuration](#configuration)

---

## Overview

LiveChat Backend provides the server-side infrastructure for a full-featured chat application. It supports group chat and private 1-to-1 messaging in real time, user authentication with JWT and refresh tokens, multi-type file uploads, and guest participation without registration.

---

## Features

- **Real-time messaging** via SignalR WebSockets
- **Group chat** — broadcast messages to all connected users
- **Private messaging** — 1-to-1 conversations with persistent chat rooms
- **JWT authentication** with access tokens (30 min) and refresh tokens (7 days)
- **Guest user support** — join chat without registering
- **File uploads** — images, audio files, and documents with automatic cleanup
- **Online presence** — live list of connected users
- **BCrypt password hashing**
- **Swagger UI** for API exploration in development
- **Docker support** with multi-stage build
- **CORS configured** for local frontend development (ports 3000, 4200, 5500)

---

## Tech Stack

| Layer | Technology |
|---|---|
| Runtime | .NET 9 / ASP.NET Core 9.0 |
| Real-time | ASP.NET Core SignalR |
| ORM | Entity Framework Core 9.0 |
| Database | SQL Server |
| Authentication | JWT Bearer (`Microsoft.AspNetCore.Authentication.JwtBearer`) |
| Password hashing | BCrypt.Net-Next |
| Object mapping | AutoMapper 15 |
| API docs | Swashbuckle / Swagger |
| Containerization | Docker (multi-stage) |

---

## Architecture

The project follows a layered architecture with clear separation of concerns:

```
HTTP / WebSocket Clients
        │
        ▼
┌─────────────────────┐
│   Controllers / Hub  │   AuthController, UploadController, ChatHub
└────────┬────────────┘
         │
         ▼
┌─────────────────────┐
│    Service Layer     │   AuthService, TokenService, UserService,
│                      │   MessageService, ChatRoomService, FileCleanupService
└────────┬────────────┘
         │
         ▼
┌─────────────────────┐
│  Repository Layer    │   UserRepository, MessageRepository,
│                      │   ChatRoomRepository, TokenRepository
└────────┬────────────┘
         │
         ▼
┌─────────────────────┐
│  Entity Framework    │   AppDBContext → SQL Server
└─────────────────────┘
```

### Key Patterns

- **Repository Pattern** — abstracts all data access behind interfaces
- **Service Layer** — business logic decoupled from controllers
- **Dependency Injection** — all services and repositories registered via `Dependencies.cs`
- **Code-First Migrations** — database schema managed through EF Core migrations
- **SignalR Hub** — stateful, bidirectional real-time communication

---

## Project Structure

```
LiveChat/
├── Controllers/
│   ├── AuthController.cs          # Login and registration endpoints
│   └── UploadController.cs        # File upload endpoints
├── Hubs/
│   └── ChatHub.cs                 # SignalR hub (group + private chat)
├── Services/
│   ├── Interfaces/                # Service contracts
│   ├── AuthService.cs             # Login / register logic
│   ├── TokenService.cs            # JWT + refresh token generation
│   ├── UserService.cs             # User creation and lookup
│   ├── MessageService.cs          # Message persistence
│   ├── ChatRoomService.cs         # Private room management
│   └── FileCleanupService.cs      # Upload directory size management
├── Repositories/
│   ├── Interfaces/                # Repository contracts
│   ├── GenericRepository.cs       # Base CRUD operations
│   ├── UserRepository.cs
│   ├── MessageRepository.cs
│   ├── ChatRoomRepository.cs
│   └── TokenRepository.cs
├── Entities/
│   ├── User.cs                    # User domain model
│   ├── ChatRoom.cs                # Chat room + participants
│   ├── Message.cs                 # Message + MessageType enum
│   └── RefreshToken.cs            # Refresh token model
├── DTOs/
│   └── AuthDTO.cs                 # Auth request/response shapes
├── Context/
│   └── AppDBContext.cs            # EF Core database context
├── IOC/
│   └── Dependencies.cs            # DI, CORS, JWT, SignalR setup
├── Migrations/                    # EF Core migration history
├── wwwroot/
│   └── uploads/
│       ├── images/
│       ├── audios/
│       └── documents/
├── appsettings.json
├── Program.cs
└── Dockerfile
```

---

## API Reference

### Authentication

#### `POST /api/auth/register`

Register a new user.

**Request body:**
```json
{
  "username": "john",
  "email": "john@example.com",
  "password": "secret123"
}
```

**Response:**
```json
{
  "accessToken": "<jwt>",
  "refreshToken": "<token>"
}
```

---

#### `POST /api/auth/login`

Authenticate an existing user.

**Request body:**
```json
{
  "email": "john@example.com",
  "password": "secret123"
}
```

**Response:**
```json
{
  "accessToken": "<jwt>",
  "refreshToken": "<token>"
}
```

---

### File Uploads

All upload endpoints require `multipart/form-data`. Files are stored under `wwwroot/uploads/` and served as static files. When the upload folder exceeds 10 files, the 5 oldest are automatically deleted.

#### `POST /api/upload/images`

Accepted formats: `.jpg`, `.jpeg`, `.png`, `.gif`, `.webp`

#### `POST /api/upload/audios`

Accepted formats: `.mp3`, `.wav`, `.ogg`, `.webm`, `.m4a`

#### `POST /api/upload/documents`

Accepted formats: `.pdf`, `.doc`, `.docx`, `.xls`, `.xlsx`, `.ppt`, `.pptx`

**Response (all upload endpoints):**
```json
{
  "url": "/uploads/images/abc123.png"
}
```

---

## SignalR Hub

**Endpoint:** `/chathub`

Authenticated users connect by passing the JWT in the query string:
```
wss://localhost:7122/chathub?access_token=<jwt>
```

Guest users connect without a token and call `RegisterUser` after connecting.

### Client → Server Methods

| Method | Parameters | Description |
|---|---|---|
| `RegisterUser` | `username: string` | Register a guest connection by username |
| `SendMessage` | `username, content, type` | Broadcast a message to all connected users |
| `SendPrivateMessage` | `senderName, recipientName, content, type` | Send a message to a specific user |

`type` corresponds to the `MessageType` enum: `Text`, `Image`, `Audio`, `Document`.

### Server → Client Events

| Event | Payload | Description |
|---|---|---|
| `UsersOnline` | `string[]` | Updated list of connected usernames |
| `ReceiveMessage` | `{ username, content, type, sentAt }` | Incoming group message |
| `ReceivePrivateMessage` | `{ username, content, type, sentAt }` | Incoming private message |

### Connection Behavior

- On connect, authenticated users are identified by their JWT claims; guest users are tracked as `Guest_<connectionId>` until they call `RegisterUser`.
- Online user list is broadcast to all clients on every connect/disconnect.
- Private conversations are scoped to a persistent `ChatRoom` identified by the two participants.
- Max message size: **10 MB**.

---

## Database Schema

### Users
| Column | Type | Notes |
|---|---|---|
| Id | Guid (PK) | |
| Username | string | |
| Email | string | |
| Password | string | BCrypt hash |
| Role | string | Default: `"User"` |
| CreatedAt | DateTime | |

### ChatRooms
| Column | Type | Notes |
|---|---|---|
| Id | Guid (PK) | |
| IsPrivate | bool | |

### ChatParticipants
| Column | Type | Notes |
|---|---|---|
| Id | int (PK) | |
| RoomId | Guid (FK) | |
| UserId | Guid? (FK) | Null for guests |
| GuestUsername | string? | Null for registered users |

### Messages
| Column | Type | Notes |
|---|---|---|
| Id | int (PK) | |
| Content | string | Text or file URL |
| SentAt | DateTime | |
| SenderId | Guid | |
| SenderUsername | string | |
| RoomId | Guid? (FK) | Null for group messages |
| Type | MessageType | Text / Image / Audio / Document |

### RefreshTokens
| Column | Type | Notes |
|---|---|---|
| Id | int (PK) | |
| Token | string | |
| ExpiresAt | DateTime | 7 days from creation |
| CreatedAt | DateTime | |
| IsRevoked | bool | |
| UserId | Guid (FK) | |

---

## Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- SQL Server instance (local or Docker)

### 1. Clone the repository

```bash
git clone https://github.com/draquio/LiveChat-Backend.git
cd LiveChat-Backend
```

### 2. Configure the database

Edit `LiveChat/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "Connection": "Server=localhost,1434;Database=LiveChat;User Id=sa;Password=YourPassword;TrustServerCertificate=True"
  },
  "Jwt": {
    "Key": "YourSuperSecretKeyAtLeast32Characters!",
    "Issuer": "LiveChatAPI",
    "Audience": "LiveChatUsers"
  }
}
```

### 3. Apply migrations

```bash
cd LiveChat
dotnet ef database update
```

### 4. Run the application

```bash
dotnet run
```

The API will be available at:
- HTTP: `http://localhost:5072`
- HTTPS: `https://localhost:7122`
- Swagger UI: `https://localhost:7122/swagger` *(development only)*

---

## Docker

### Build and run with Docker

```bash
docker build -t livechat-backend ./LiveChat
docker run -p 8080:8080 -p 8081:8081 livechat-backend
```

The Dockerfile uses a **multi-stage build**:
1. **Base** — `mcr.microsoft.com/dotnet/aspnet:9.0` runtime image
2. **Build** — `mcr.microsoft.com/dotnet/sdk:9.0` to compile and publish
3. **Final** — minimal runtime image running `dotnet LiveChat.dll`

Exposed ports: `8080` (HTTP), `8081` (HTTPS).

---

## Configuration

| Key | Default | Description |
|---|---|---|
| `ConnectionStrings:Connection` | `localhost,1434` | SQL Server connection string |
| `Jwt:Key` | *(required)* | Secret key for signing JWTs (min 32 chars) |
| `Jwt:Issuer` | `LiveChatAPI` | JWT issuer claim |
| `Jwt:Audience` | `LiveChatUsers` | JWT audience claim |

### CORS

By default, the following origins are allowed (credentials enabled):

- `http://localhost:3000`
- `http://localhost:4200`
- `http://127.0.0.1:5500`

Update the `AllowLocalhost` policy in `IOC/Dependencies.cs` to add or change allowed origins.

---

## License

This project is open source and available under the [MIT License](LICENSE).
