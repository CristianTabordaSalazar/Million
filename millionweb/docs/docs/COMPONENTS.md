# Components Catalog

## <FiltersForm />
- **Location**: `src/components/FiltersForm.tsx`
- **Role**: Controlled filter form with debounce, reset page to 1 on changes.
- **Props**:
  - `initial: Filters`
  - `onChange: (next: Filters) => void`
- **Events**:
  - Submit: emits `onChange` with page=1
  - Clear: resets to initial filters

## <PropertiesTable />
- **Location**: `src/components/PropertiesTable.tsx`
- **Role**: Render property rows with key columns.
- **Props**: `data: Property[]`

## <usePropertyDetail />
- **Location**: `src/components/usePropertyDetail.tsx`
- **Role**: Show detailed property info (owner, trace, etc).
- **Props**: `data: PropertyDetail`

## <ReactQueryProvider />
- Provides `QueryClient` and global configuration.
