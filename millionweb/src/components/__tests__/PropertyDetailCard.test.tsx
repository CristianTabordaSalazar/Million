import { render, screen, within } from '@testing-library/react';
import PropertyDetailCard from '../PropertyDetailCard';
import { describe, expect, test } from 'vitest';

function makeData(overrides: Partial<any> = {}) {
  return {
    id: 'p1',
    firstImageUrl: 'https://example.com/img.jpg',
    name: 'Casa Azul',
    address: 'Av. 123 #45-67',
    price: 250000,
    codeInternal: 'ABC-001',
    year: 2020,
    owner: {
      id: '11111111-1111-1111-1111-111111111111',
      name: 'Juan Pérez',
      address: 'Calle 9',
      dateOfBirth: '1990-05-10T00:00:00.000Z',
      photo: 'https://example.com/owner.jpg',
    },
    traces: [
      { id: 't1', dateSale: '2024-01-01T12:34:00.000Z', name: 'Compra inicial', value: 120000, tax: 3000 },
      { id: 't2', dateSale: '2024-02-15T09:00:00.000Z', name: 'Reforma', value: 80000, tax: 1000 },
    ],
    ...overrides,
  };
}

// Helper para afirmar/convertir a HTMLElement de forma segura
function asHTMLElement(el: Element | null): HTMLElement {
  if (!(el instanceof HTMLElement)) {
    throw new Error('Elemento no es un HTMLElement (o es null).');
  }
  return el;
}

describe('PropertyDetailCard', () => {
  test('muestra datos base, imagen y owner con foto', () => {
    const data = makeData();
    render(<PropertyDetailCard data={data} />);

    // Card base (scope seguro a HTMLElement)
    const baseHeading = screen.getByRole('heading', { name: data.name });
    const baseCard = asHTMLElement(baseHeading.closest('.card'));

    // Imagen principal
    const img = within(baseCard).getByRole('img', { name: data.name });
    expect(img).toHaveAttribute('src', data.firstImageUrl);

    // Address de la card base (scope del <p>)
    const addrStrong = within(baseCard).getByText(/address:/i);
    const addrP = asHTMLElement(addrStrong.closest('p'));
    expect(within(addrP).getByText(/av\. 123/i)).toBeInTheDocument();

    // Otros campos base
    expect(within(baseCard).getByText(/\$250,000\.00/)).toBeInTheDocument();
    expect(within(baseCard).getByText(/abc-001/i)).toBeInTheDocument();
    expect(within(baseCard).getByText('2020')).toBeInTheDocument();

    // Card de Owner
    const ownerHeading = screen.getByRole('heading', { name: /owner/i });
    const ownerCard = asHTMLElement(ownerHeading.closest('.card'));

    const ownerImg = within(ownerCard).getByRole('img', { name: /juan pérez/i });
    expect(ownerImg).toHaveAttribute('src', data.owner.photo);

    const ownerAddrStrong = within(ownerCard).getByText(/address:/i);
    const ownerAddrP = asHTMLElement(ownerAddrStrong.closest('p'));
    expect(within(ownerAddrP).getByText(/calle 9/i)).toBeInTheDocument();

    // Tabla de trazas: 1 header + 2 filas
    const table = screen.getByRole('table');
    const rows = within(table).getAllByRole('row');
    expect(rows).toHaveLength(3);
    expect(screen.getByText(/compra inicial/i)).toBeInTheDocument();
    expect(screen.getByText(/\$120,000\.00/)).toBeInTheDocument();
    expect(screen.getByText(/reforma/i)).toBeInTheDocument();
    expect(screen.getByText(/\$80,000\.00/)).toBeInTheDocument();
  });

  test('muestra placeholders cuando no hay imagen y no hay owner', () => {
    const EMPTY_GUID = '00000000-0000-0000-0000-000000000000';
    const data = makeData({ firstImageUrl: undefined, owner: { id: EMPTY_GUID }, traces: [] });
    render(<PropertyDetailCard data={data} />);
    expect(screen.getByText(/no image/i)).toBeInTheDocument();
    expect(screen.getByText(/no owner linked\./i)).toBeInTheDocument();
    expect(screen.getByText(/no traces\./i)).toBeInTheDocument();
  });
});
