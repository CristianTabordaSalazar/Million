'use client';
import { useMemo } from 'react';
import Link from 'next/link';
import {
  ColumnDef,
  getCoreRowModel,
  useReactTable,
  flexRender,
} from '@tanstack/react-table';
import type { Property } from '@/lib/schemas';

declare module '@tanstack/react-table' {
  interface ColumnMeta<TData, TValue> {
    thClassName?: string;
    tdClassName?: string;
  }
}

type Props = { data: Property[] };

export default function PropertiesTable({ data }: Props) {
  const columns = useMemo<ColumnDef<Property>[]>(
    () => [
      {
        header: 'Name',
        accessorKey: 'name',
        meta: {
          thClassName: '',
          tdClassName: 'text-truncate',
        },
        cell: ({ getValue }) => (
          <span className="d-inline-block text-truncate" style={{ maxWidth: 200 }}>
            {String(getValue() ?? '')}
          </span>
        ),
      },
      {
        header: 'Address',
        accessorKey: 'address',
        meta: {
          thClassName: 'd-none d-md-table-cell',
          tdClassName: 'd-none d-md-table-cell text-truncate',
        },
        cell: ({ getValue }) => (
          <span className="d-inline-block text-truncate">
            {String(getValue() ?? '')}
          </span>
        ),
      },
      {
        header: 'Price',
        accessorKey: 'price',
        meta: {
          thClassName: 'd-none d-sm-table-cell',
          tdClassName: 'd-none d-sm-table-cell',
        },
        cell: ({ getValue }) =>
          Number(getValue() ?? 0).toLocaleString('en-US', {
            style: 'currency',
            currency: 'USD',
          }),
      },
      {
        header: 'Year',
        accessorKey: 'year',
        meta: {
          thClassName: 'd-none d-lg-table-cell text-center',
          tdClassName: 'd-none d-lg-table-cell text-center',
        },
      },
      {
        header: 'Action',
        id: 'action',
        meta: {
          thClassName: 'text-end action-sticky',
          tdClassName: 'text-end action-sticky',
        },
        cell: ({ row }) => (
          <div className="btn-group">
            <Link
              href={`/properties/${row.original.id}`}
              className="btn btn-sm btn-primary text-nowrap"
              aria-label="View"
            >
              <span className="d-inline d-sm-none" aria-hidden>
                View
              </span>
              <span className="d-none d-sm-inline">View</span>
            </Link>
          </div>
        ),
      },
    ],
    []
  );

  const table = useReactTable({ data, columns, getCoreRowModel: getCoreRowModel() });

  return (
    <div className="table-responsive">
      <table className="table table-hover align-middle">
        <thead className="table-light">
          {table.getHeaderGroups().map(hg => (
            <tr key={hg.id}>
              {hg.headers.map(h => {
                const thClass = h.column.columnDef.meta?.thClassName ?? '';
                return (
                  <th key={h.id} className={thClass}>
                    {flexRender(h.column.columnDef.header, h.getContext())}
                  </th>
                );
              })}
            </tr>
          ))}
        </thead>
        <tbody>
          {table.getRowModel().rows.map(r => (
            <tr key={r.id}>
              {r.getVisibleCells().map(c => {
                const tdClass = c.column.columnDef.meta?.tdClassName ?? '';
                return (
                  <td key={c.id} className={tdClass}>
                    {flexRender(c.column.columnDef.cell, c.getContext())}
                  </td>
                );
              })}
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
