# TodoList API

REST API para gestionar tareas y usuarios, construida con .NET 10, Clean Architecture y PostgreSQL.

## Arquitectura

```
todoList.Domain          → Entidades y contratos (interfaces)
todoList.Application     → Casos de uso y DTOs
todoList.Infrastructure  → Persistencia (EF Core), servicios (BCrypt, JWT)
todoList.Presentation    → Controllers, middleware, configuración
todoList.Tests           → Pruebas unitarias (xUnit + Moq)
```

## Endpoints

### Autenticación

| Método | Ruta | Descripción | Auth |
|--------|------|-------------|------|
| `POST` | `/api/login` | Iniciar sesión, retorna JWT | No |

### Usuarios

| Método | Ruta | Descripción | Auth |
|--------|------|-------------|------|
| `POST` | `/api/usuarios/crear` | Crear usuario | No |
| `GET` | `/api/usuarios/all` | Obtener todos los usuarios | Admin |
| `PATCH` | `/api/usuarios/{id}/activar` | Activar usuario | Admin |
| `PATCH` | `/api/usuarios/{id}/desactivar` | Desactivar usuario | Admin |
| `PATCH` | `/api/usuarios/{id}/cambiar-nombre` | Cambiar nombre | Owner/Admin |
| `PATCH` | `/api/usuarios/{id}/cambiar-contraseña` | Cambiar contraseña | Owner/Admin |
| `DELETE` | `/api/usuarios/{id}/borrar` | Eliminar usuario | Admin |

### Tareas

| Método | Ruta | Descripción | Auth |
|--------|------|-------------|------|
| `GET` | `/api/tareas` | Obtener todas las tareas | Sí |
| `GET` | `/api/tareas/{id}` | Obtener tarea por ID | Sí |
| `POST` | `/api/tareas` | Crear tarea | Sí |
| `PATCH` | `/api/tareas/{id}/nombre` | Cambiar nombre de tarea | Sí |
| `POST` | `/api/tareas/{id}/completar` | Marcar tarea como completada | Sí |
| `DELETE` | `/api/tareas/{id}` | Eliminar tarea | Sí |

### Salud

| Método | Ruta | Descripción | Auth |
|--------|------|-------------|------|
| `GET` | `/health` | Health check | No |

## Body Requests

### Login
```json
POST /api/login
{
  "email": "usuario@ejemplo.com",
  "password": "MiPassword123!"
}
```

### Crear Usuario
```json
POST /api/usuarios/crear
{
  "nombre": "Juan Pérez",
  "correo": "juan@ejemplo.com",
  "password": "MiPassword123!"
}
```

### Crear Tarea
```json
POST /api/tareas
{
  "nombre": "Mi nueva tarea",
  "usuario_id": "guid-del-usuario"
}
```

### Cambiar Nombre (Tarea)
```json
PATCH /api/tareas/{id}/nombre
{
  "id": "guid-de-la-tarea",
  "nuevoNombre": "Nombre actualizado"
}
```

### Cambiar Nombre (Usuario)
```json
PATCH /api/usuarios/{id}/cambiar-nombre
{
  "id": "guid-del-usuario",
  "nuevoNombre": "Nuevo Nombre"
}
```

### Cambiar Contraseña
```json
PATCH /api/usuarios/{id}/cambiar-contraseña
{
  "id": "guid-del-usuario",
  "nuevaPassword": "NuevaPassword123!"
}
```

## Respuestas

### Exitosa (200/201)
```json
{
  "id": "guid",
  "nombre": "string",
  "isCompleted": false,
  "fechaActualizacion": "2026-01-01T00:00:00Z"
}
```

### Error
```json
{
  "error": "Mensaje descriptivo del error"
}
```

### Códigos de estado
- **200** - OK
- **201** - Created
- **204** - No Content
- **400** - Bad Request (datos inválidos)
- **401** - Unauthorized (no autenticado)
- **403** - Forbidden (sin permisos)
- **404** - Not Found
- **409** - Conflict (duplicado)

## Desarrollo

### Requisitos
- .NET 10 SDK
- Docker (opcional, para PostgreSQL)

### Ejecutar en desarrollo
```bash
dotnet run --project todoList.Presentation
```

La API usa **InMemory Database** automáticamente en Development. Credenciales del seed:
- **Admin:** `admin@dev.local` / `Admin123!`
- **Usuario:** `user@dev.local` / `User123!`

### Ejecutar pruebas
```bash
dotnet test todoList.Tests/todoList.Tests.csproj
```

### Desplegar con Docker
```bash
docker compose up -d
```

## Variables de entorno (Producción)

| Variable | Descripción |
|----------|-------------|
| `ConnectionStrings__DefaultConnection` | Cadena de conexión PostgreSQL |
| `Jwt__SecretKey` | Clave secreta JWT (mín. 32 chars) |
| `Jwt__Issuer` | Emisor del token |
| `Jwt__Audience` | Audiencia del token |
| `Cors__AllowedOrigins__0` | Orígenes permitidos para CORS |

## Tecnologías

- .NET 10
- Entity Framework Core 10 (PostgreSQL / InMemory)
- BCrypt.Net (hashing de contraseñas)
- JWT Bearer Authentication
- xUnit + Moq + FluentAssertions (testing)
