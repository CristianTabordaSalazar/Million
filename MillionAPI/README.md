# Million API - Backend
This is the backend service for the Million API project, which provides a RESTful API for managing and accessing various resources.

## Features
- .Net Core 9
- Mongo DB
- Clean Architecture

## Environment Variables
- `MONGO_CONN` — connection string, e.g. `mongodb://localhost:27017`
- `MONGO_DB` — database name, e.g. `milliondb`

## Run
```bash
dotnet restore
dotnet run --project src/MillionApi

Swagger:
- UI: http://localhost:5211/swagger
- JSON: http://localhost:5211/swagger/v1/swagger.json

Key Endpoints
-------------
- GET /api/properties — list with filters + pagination
- GET /api/properties/{id}/detail — detail
- GET /api/properties/{id}
- GET /api/properties/{name}

Filters and pagination

Pagination: page, pageSize (por defecto page=1, pageSize=10).

Filters: name, address, minPrice, maxPrice.

Response:
{
  "items": [ /* ... */ ],
  "total": 123,
  "page": 1,
  "pageSize": 10
}

Errors

Standard format:

{
  "error": "BadRequest",
  "message": "Detalle del problema",
  "traceId": "00-...-..."
}

Licencia

MIT


---

# 2) Swagger + XML comments (habilitar en tu API .NET)

### a) Instala Swashbuckle
```bash
dotnet add src/MillionApi package Swashbuckle.AspNetCore