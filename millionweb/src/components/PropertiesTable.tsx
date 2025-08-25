'use client';
import { useMemo } from 'react';
import { ColumnDef, getCoreRowModel, useReactTable, flexRender } from '@tanstack/react-table';
import type { Property } from '@/lib/schemas';
import Link from 'next/link';

type Props = {
  data: Property[];
};

export default function PropertiesTable({ data }: Props) {
  const columns = useMemo<ColumnDef<Property>[]>(() => [
    { header: 'Name', accessorKey: 'name' },
    { header: 'Address', accessorKey: 'address' },
    { header: 'Price', accessorKey: 'price',
      cell: info => info.getValue<number>().toLocaleString('en-US', { style: 'currency', currency: 'USD' })
    },
    { header: 'Year', accessorKey: 'year' },
    {
      header: 'Action',
      cell: ({ row }) => <Link className="btn btn-sm btn-outline-primary" href={`/properties/${row.original.id}`}>View</Link>
    },
  ], []);

  const table = useReactTable({ data, columns, getCoreRowModel: getCoreRowModel() });

  return (
    <div className="table-responsive">
      <table className="table table-hover align-middle">
        <thead className="table-light">
          {table.getHeaderGroups().map(hg => (
            <tr key={hg.id}>
              {hg.headers.map(h => (
                <th key={h.id}>{flexRender(h.column.columnDef.header, h.getContext())}</th>
              ))}
            </tr>
          ))}
        </thead>
        <tbody>
          {table.getRowModel().rows.map(r => (
            <tr key={r.id}>
              {r.getVisibleCells().map(c => (
                <td key={c.id}>{flexRender(c.column.columnDef.cell, c.getContext())}</td>
              ))}
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
