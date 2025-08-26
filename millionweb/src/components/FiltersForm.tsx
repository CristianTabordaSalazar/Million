import { useEffect, useMemo, useState } from 'react';
import { FiltersSchema } from '@/lib/schemas';

type Filters = {
  name?: string;
  address?: string;
  minPrice?: number;
  maxPrice?: number;
  page: number;
  pageSize: number;
};

function isFiniteNumber(n: unknown): n is number {
  return typeof n === 'number' && Number.isFinite(n);
}

function normalizeNumbers<T extends Partial<Filters>>(x: T): T {
  const out: any = { ...x };
  if (!isFiniteNumber(out.minPrice)) out.minPrice = undefined;
  if (!isFiniteNumber(out.maxPrice)) out.maxPrice = undefined;
  return out;
}

export default function FiltersForm(props: {
  initial: Partial<Filters>;
  onChange: (f: Filters) => void;
}) {
  const { initial, onChange } = props;

  const [name, setName] = useState(initial.name ?? '');
  const [address, setAddress] = useState(initial.address ?? '');
  const [minPrice, setMinPrice] = useState<string>('');
  const [maxPrice, setMaxPrice] = useState<string>('');
  const [pageSize, setPageSize] = useState<number>(initial.pageSize ?? 10);

  const basePayload = useMemo<Filters>(() => {
    const raw: Partial<Filters> = {
      name: name || undefined,
      address: address || undefined,
      minPrice: minPrice === '' ? undefined : parseInt(minPrice, 10),
      maxPrice: maxPrice === '' ? undefined : parseInt(maxPrice, 10),
      page: 1,
      pageSize,
    };
    return normalizeNumbers(raw) as Filters;
  }, [name, address, minPrice, maxPrice, pageSize]);

  useEffect(() => {
    const t = setTimeout(() => {
      try {
        const parsed = FiltersSchema.parse(basePayload);
        onChange(parsed);
      } catch {
      }
    }, 400);
    return () => clearTimeout(t);
  }, [basePayload, onChange]);

  function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    try {
      const parsed = FiltersSchema.parse(basePayload);
      onChange(parsed);
    } catch {
    }
  }

  function handleClear() {
    setName('');
    setAddress('');
    setMinPrice('');
    setMaxPrice('');
    try {
      const parsed = FiltersSchema.parse({ page: 1, pageSize });
      onChange(parsed as Filters);
    } catch {
    }
  }

  return (
    <form className="row g-3 mb-3" onSubmit={handleSubmit}>
      <div className="col-12 col-md-4">
        <label className="form-label" htmlFor="f-name">Name</label>
        <input
          id="f-name"
          name="name"
          className="form-control"
          placeholder="e.g. Blue House"
          value={name}
          onChange={(e) => setName(e.target.value)}
        />
      </div>

      <div className="col-12 col-md-4">
        <label className="form-label" htmlFor="f-address">Address</label>
        <input
          id="f-address"
          name="address"
          className="form-control"
          placeholder="e.g. 123 Street"
          value={address}
          onChange={(e) => setAddress(e.target.value)}
        />
      </div>

      <div className="col-6 col-md-2">
        <label className="form-label" htmlFor="f-minPrice">Min price</label>
        <input
          id="f-minPrice"
          name="minPrice"
          className="form-control"
          type="number"
          value={minPrice}
          onChange={(e) => setMinPrice(e.target.value)}
        />
      </div>

      <div className="col-6 col-md-2">
        <label className="form-label" htmlFor="f-maxPrice">Max price</label>
        <input
          id="f-maxPrice"
          name="maxPrice"
          className="form-control"
          type="number"
          value={maxPrice}
          onChange={(e) => setMaxPrice(e.target.value)}
        />
      </div>

      <div className="col-6 col-md-2">
        <label className="form-label" htmlFor="f-pageSize">Page size</label>
        <select
          id="f-pageSize"
          name="pageSize"
          className="form-select"
          value={pageSize}
          onChange={(e) => setPageSize(parseInt(e.target.value, 10))}
        >
          <option value="10">10</option>
          <option value="20">20</option>
          <option value="50">50</option>
        </select>
      </div>

      <div className="col-12 d-flex gap-2">
        <button className="btn btn-primary" type="submit">Search</button>
        <button className="btn btn-outline-secondary" type="button" onClick={handleClear}>
          Clear
        </button>
      </div>
    </form>
  );
}