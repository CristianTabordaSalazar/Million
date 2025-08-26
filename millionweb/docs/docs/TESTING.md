# Testing Guide

Run tests:
```
pnpm test
```

## Current Tests
- FiltersForm.test.tsx
  - Triggers onChange with debounce
  - Submit sets page=1
  - Clear resets filters
- PropertiesTable.test.tsx
  - Renders table rows and columns
- PropertyDetailCard.test.tsx
  - Renders base data, image, and owner
  - Displays placeholders when there is no image and no owner

## Notes
- Use fake timers for debounce (`vi.useFakeTimers()`).
- Mock fetch/React Query to avoid backend dependency.