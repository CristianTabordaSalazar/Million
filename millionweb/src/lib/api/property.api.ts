import { PropertySchema } from '@/lib/schemas';
import { safeJson } from './helpers';

const BASE = process.env.NEXT_PUBLIC_API_BASE!;

export async function fetchProperties(params: {
  name?: string;
  address?: string;
  minPrice?: number;
  maxPrice?: number;
  page?: number;
  pageSize?: number;
}): Promise<{ items: unknown[]; total: number }> {
  const q = new URLSearchParams();
  if (params.name) q.set('name', params.name);
  if (params.address) q.set('address', params.address);
  if (params.minPrice != null && !Number.isNaN(params.minPrice)) q.set('minPrice', String(params.minPrice));
  if (params.maxPrice != null && !Number.isNaN(params.maxPrice)) q.set('maxPrice', String(params.maxPrice));
  if (params.page) q.set('page', String(params.page));
  if (params.pageSize) q.set('pageSize', String(params.pageSize));

  const res = await fetch(`${BASE}/properties?${q.toString()}`, { cache: 'no-store' });
  if (!res.ok) throw new Error('Error fetching properties');

  const payload = await safeJson<{ items: unknown[]; total: number }>(res);
  payload.items = payload.items.map((p) => PropertySchema.parse(p));
  return payload;
}

export async function fetchPropertyById(id: string) {
  const res = await fetch(`${BASE}/properties/${id}`, { cache: 'no-store' });
  if (!res.ok) throw new Error('Property not found');

  const data = await safeJson<unknown>(res);
  return PropertySchema.parse(data);
}
