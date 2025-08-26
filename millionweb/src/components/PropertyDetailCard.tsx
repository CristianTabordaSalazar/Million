'use client';
import { PropertyDetailResponse } from '@/lib/schemas/propertyDetailSchema.schema';
import { format } from 'date-fns';
import { es } from 'date-fns/locale';

type Props = { data: PropertyDetailResponse };

export default function PropertyDetailCard({ data }: Props) {
  // Si tu API retorna un GUID vacío cuando no hay owner
  const ownerExists =
    !!data.owner &&
    !!data.owner.id &&
    data.owner.id !== '00000000-0000-0000-0000-000000000000';

  return (
    <div className="row g-4">
      {/* Imagen + datos base */}
      <div className="col-12 col-md-5">
        <div className="card h-100">
          {data.firstImageUrl ? (
            <img
              src={data.firstImageUrl}
              alt={data.name}
              className="card-img-top"
              style={{ objectFit: 'cover', maxHeight: 320 }}
            />
          ) : (
            <div
              className="d-flex align-items-center justify-content-center bg-light"
              style={{ height: 320 }}
            >
              <span className="text-muted">No image</span>
            </div>
          )}
          <div className="card-body">
            <h5 className="card-title mb-2">{data.name}</h5>
            <p className="mb-1">
              <strong>Address:</strong> {data.address}
            </p>
            <p className="mb-1">
              <strong>Price:</strong>{' '}
              {data.price.toLocaleString('en-US', {
                style: 'currency',
                currency: 'USD',
              })}
            </p>
            <p className="mb-1">
              <strong>Code:</strong> {data.codeInternal}
            </p>
            <p className="mb-0">
              <strong>Year:</strong> {data.year}
            </p>
          </div>
        </div>
      </div>

      {/* Owner */}
      <div className="col-12 col-md-7">
        <div className="card h-100">
          <div className="card-body">
            <h5 className="card-title">Owner</h5>
            {ownerExists ? (
              <div className="d-flex gap-3">
                {data.owner.photo ? (
                  <img
                    src={data.owner.photo}
                    alt={data.owner.name}
                    width={96}
                    height={96}
                    style={{ objectFit: 'cover', borderRadius: 8 }}
                  />
                ) : (
                  <div
                    className="bg-light d-flex align-items-center justify-content-center"
                    style={{ width: 96, height: 96, borderRadius: 8 }}
                  >
                    <span className="text-muted">No photo</span>
                  </div>
                )}
                <div>
                  <p className="mb-1">
                    <strong>Name:</strong> {data.owner.name}
                  </p>
                  <p className="mb-1">
                    <strong>Address:</strong> {data.owner.address}
                  </p>
                  <p className="mb-0">
                    <strong>Birth:</strong>{' '}
                    {data.owner.dateOfBirth
                      ? format(new Date(data.owner.dateOfBirth), 'PPP', {
                          locale: es,
                        })
                      : '—'}
                  </p>
                </div>
              </div>
            ) : (
              <p className="text-muted mb-0">No owner linked.</p>
            )}
          </div>
        </div>
      </div>

      {/* Traces */}
      <div className="col-12">
        <div className="card">
          <div className="card-body">
            <h5 className="card-title">Property Traces</h5>
            {data.traces.length === 0 ? (
              <p className="text-muted mb-0">No traces.</p>
            ) : (
              <div className="table-responsive">
                <table className="table table-sm align-middle">
                  <thead>
                    <tr>
                      <th style={{ whiteSpace: 'nowrap' }}>Date</th>
                      <th>Name</th>
                      <th className="text-end">Value</th>
                      <th className="text-end">Tax</th>
                    </tr>
                  </thead>
                  <tbody>
                    {data.traces.map((t) => (
                      <tr key={t.id}>
                        <td style={{ whiteSpace: 'nowrap' }}>
                          {format(new Date(t.dateSale), 'Pp', { locale: es })}
                        </td>
                        <td>{t.name}</td>
                        <td className="text-end">
                          {t.value.toLocaleString('en-US', {
                            style: 'currency',
                            currency: 'USD',
                          })}
                        </td>
                        <td className="text-end">
                          {t.tax.toLocaleString('en-US', {
                            style: 'currency',
                            currency: 'USD',
                          })}
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            )}
          </div>
        </div>
      </div>
    </div>
  );
}
