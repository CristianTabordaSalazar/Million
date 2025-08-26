'use client';
import { useParams, useRouter } from 'next/navigation';
import { usePropertyDetail } from '@/hooks/useProperties';
import PropertyDetailCard from '@/components/PropertyDetailCard';

export default function PropertyDetailPage() {
  const { id } = useParams<{ id: string }>();
  const router = useRouter();

  const { data, isLoading, isError, refetch, isFetching } = usePropertyDetail(id);

  if (isLoading) return <div className="container py-4">Loading…</div>;

  if (isError || !data) {
    return (
      <div className="container py-4">
        <div className="alert alert-danger d-flex justify-content-between align-items-center">
          <span>Property not found</span>
          <button
            className="btn btn-sm btn-light"
            onClick={() => refetch()}
            disabled={isFetching}
          >
            Retry
          </button>
        </div>
        <button className="btn btn-secondary" onClick={() => router.push('/properties')}>
          Back
        </button>
      </div>
    );
  }

  return (
    <div className="container py-4">
      <button className="btn btn-link p-0 mb-3" onClick={() => router.back()}>
        &laquo; Back
      </button>
      <PropertyDetailCard data={data} />
    </div>
  );
}
