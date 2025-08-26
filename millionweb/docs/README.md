# Million Property System — Frontend (Next.js)

Frontend application to browse and manage properties.
This project is part of a **Full‑Stack Technical Test** and demonstrates clean architecture, React, and proper error handling.

## Features
- Property listing with filters (name, address, price range)
- Pagination (server‑side driven)
- Detail view per property with retry/back options
- Responsive UI styled with Bootstrap 5
- Data fetching and caching powered by React Query
- Strong contracts enforced with Zod + TypeScript schemas
- Unit tests for core components

## Project Structure
```
src/
  app/
    page.tsx
    properties/
      page.tsx
      [id]/page.tsx
  components/
    FiltersForm.tsx
    PropertiesTable.tsx
    PropertyDetailCard.tsx
    ReactQueryProvider.tsx
  hooks/
    useProperties.ts
  lib/
    api/
    schemas/
  components/__tests__/
```

## Getting Started

### Requirements
- Node.js 20+
- NPM
- Backend (ASP.NET Core 9.0) running with MongoDB

### Environment Variables
Create `.env.local` in the project root:
```
NEXT_PUBLIC_API_BASE=http://localhost:5211
```

### Install & Run
```bash
npm install
npm run dev
```

Visit: [http://localhost:3000](http://localhost:3000)

## Testing
- Tests located in `src/components/__tests__/`
- Run with `npm run test`

## Documentation
- [Architecture](docs/ARCHITECTURE.md)
- [Components](docs/COMPONENTS.md)
- [API](docs/API.md)
- [Testing](docs/TESTING.md)

---
**Author:** Cristian Taborda
**Stack:** Next.js • React • ASP.NET Core • MongoDB
