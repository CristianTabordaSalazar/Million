'use client';
import { useState } from 'react';
import FiltersForm from '@/components/FiltersForm';
import PropertiesTable from '@/components/PropertiesTable';
import { useProperties } from '@/hooks/useProperties';
import type { Filters } from '@/lib/schemas';

const initialFilters: Filters = { page: 1, pageSize: 10 };

export default function PropertiesPage() {
  const [filters, setFilters] = useState<Filters>(initialFilters);
  const { data, isLoading, isError } = useProperties(filters);
  console.log(data);

  const total = data?.total ?? 0;
  const items = (data?.items as any[]) ?? [];
  const totalPages = Math.max(1, Math.ceil(total / (filters.pageSize ?? 10)));

  return (
    <div className="container py-4">
      <h1 className="mb-3">Properties</h1>

      <FiltersForm initial={initialFilters} onChange={setFilters} />

      {isLoading && <div className="alert alert-info">Loading…</div>}
      {isError && <div className="alert alert-danger">Error loading properties</div>}

      {!isLoading && !isError && (
        <>
          <div className="d-flex justify-content-between align-items-center mb-2">
            <div className="text-muted">Total: {total}</div>
            <div className="btn-group">
              <button disabled={filters.page <= 1} className="btn btn-outline-secondary btn-sm"
                onClick={() => setFilters(f => ({ ...f, page: (f.page ?? 1) - 1 }))}>
                « Prev
              </button>
              <span className="btn btn-outline-secondary btn-sm disabled">
                Page {filters.page} / {totalPages}
              </span>
              <button disabled={filters.page >= totalPages} className="btn btn-outline-secondary btn-sm"
                onClick={() => setFilters(f => ({ ...f, page: (f.page ?? 1) + 1 }))}>
                Next »
              </button>
            </div>
          </div>

          <PropertiesTable data={items as any} />
        </>
      )}
    </div>
  );
}
