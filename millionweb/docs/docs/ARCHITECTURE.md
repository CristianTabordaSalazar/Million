# Frontend Architecture

## Layers
- **app/**: Routes (Next.js App Router). Each `page.tsx` orchestrates hooks and components.
- **components/**: Presentational components. No direct API calls, receive typed props.
- **hooks/**: Data fetching logic (React Query). Exposes `useProperties`, `usePropertyDetail`.
- **lib/api/**: API client (fetch, helpers, parsing).
- **lib/schemas/**: Type and validation with Zod/TypeScript.

## Error Handling
- Loading: show "Loading…"
- Error: show Bootstrap alert + retry/back

## Styling
- Bootstrap classes (`.container`, `.row`, `.col-*`).

## Decisions
- React Query for caching/retries.
- Zod schemas for strong contracts.