'use client';
import { useProperty } from '@/hooks/useProperties';
import { useParams, useRouter } from 'next/navigation';

export default function PropertyDetailPage() {
  const { id } = useParams<{ id: string }>();
  const router = useRouter();
  const { data, isLoading, isError } = useProperty(id);

  if (isLoading) return <div className="container py-4">Loading…</div>;
  if (isError || !data) return (
    <div className="container py-4">
      <div className="alert alert-danger">Property not found</div>
      <button className="btn btn-secondary" onClick={() => router.push('/properties')}>Back</button>
    </div>
  );

  return (
    <div className="container py-4">
      <button className="btn btn-link p-0 mb-3" onClick={() => router.back()}>&laquo; Back</button>
      <div className="card shadow-sm">
        <div className="card-body">
          <h2 className="card-title">{data.name}</h2>
          <p className="card-text text-muted">{data.address}</p>
          <p className="card-text fw-bold">{data.price.toLocaleString('en-US', { style: 'currency', currency: 'USD' })}</p>
          {data.year && <p className="card-text">Year: {data.year}</p>}
          {data.codeInternal && <p className="card-text"><small className="text-muted">Code: {data.codeInternal}</small></p>}
        </div>
      </div>
    </div>
  );
}
