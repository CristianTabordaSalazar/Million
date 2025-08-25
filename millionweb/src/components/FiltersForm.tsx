'use client';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { FiltersSchema, type FiltersIn, type Filters } from '@/lib/schemas';
import { useEffect } from 'react';

type Props = {
  initial: Partial<FiltersIn>;
  onChange: (f: Filters) => void;
};

export default function FiltersForm({ initial, onChange }: Props) {
  const { register, handleSubmit, watch, setValue, formState: { errors } } =
    useForm<FiltersIn>({
      defaultValues: initial,
      resolver: zodResolver(FiltersSchema),
    });

  const all = watch();
  useEffect(() => {
    const t = setTimeout(() => {
      const parsed = FiltersSchema.parse({ ...all, page: 1 });
      onChange(parsed);
    }, 400);
    return () => clearTimeout(t);
  }, [all.name, all.address, all.minPrice, all.maxPrice, all.pageSize]);

  return (
    <form
      className="row g-3 mb-3"
      onSubmit={handleSubmit((raw) => {
        const parsed = FiltersSchema.parse({ ...raw, page: 1 });
        onChange(parsed);
      })}
    >
      <div className="col-12 col-md-4">
        <label className="form-label">Name</label>
        <input className="form-control" placeholder="e.g. Casa Azul" {...register('name')} />
        {errors.name && <div className="text-danger small">{errors.name.message}</div>}
      </div>

      <div className="col-12 col-md-4">
        <label className="form-label">Address</label>
        <input className="form-control" placeholder="e.g. Calle 123" {...register('address')} />
      </div>

      <div className="col-6 col-md-2">
        <label className="form-label">Min price</label>
        <input
          type="number"
          className="form-control"
          {...register('minPrice', { valueAsNumber: true })}
        />
      </div>

      <div className="col-6 col-md-2">
        <label className="form-label">Max price</label>
        <input
          type="number"
          className="form-control"
          {...register('maxPrice', { valueAsNumber: true })}
        />
      </div>

      <div className="col-6 col-md-2">
        <label className="form-label">Page size</label>
        <select className="form-select" {...register('pageSize', { valueAsNumber: true })}>
          {[10, 20, 50].map(n => <option key={n} value={n}>{n}</option>)}
        </select>
      </div>

      <div className="col-12 d-flex gap-2">
        <button type="submit" className="btn btn-primary">Search</button>
        <button
          type="button"
          className="btn btn-outline-secondary"
          onClick={() => {
            setValue('name', undefined);
            setValue('address', undefined);
            setValue('minPrice', undefined);
            setValue('maxPrice', undefined);
            const parsed = FiltersSchema.parse({ page: 1, pageSize: 10 });
            onChange(parsed);
          }}
        >
          Clear
        </button>
      </div>
    </form>
  );
}
