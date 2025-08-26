# API Contracts

Base URL: `NEXT_PUBLIC_API_BASE`

## GET /api/properties
Query params: `name`, `address`, `minPrice`, `maxPrice`, `page`, `pageSize`.

Response:
```json
{
  "items": [{ "id": "1", "name": "Blue House", "address": "123 Main St", "price": 120000 }],
  "total": 57
}
```

## GET /api/properties/{id}
Response:
```json
{
  "id": "1",
  "name": "Blue House",
  "address": "123 Main St",
  "price": 120000,
  "owner": { "name": "Alice" },
  "images": [{ "url": "https://..." }],
  "trace": [{ "status": "Created", "at": "2025-08-20T10:00:00Z" }]
}
```

Errors:
- 404 Not Found → show "Property not found"
- 500/Network → show alert
