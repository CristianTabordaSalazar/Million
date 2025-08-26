import { render, screen, within } from '@testing-library/react';
import { describe, expect, test } from 'vitest';
import PropertiesTable from '../PropertiesTable';


const data = [
  { id: 'p1', name: 'Casa Verde', address: 'Calle 1', price: 100000, year: 2010 },
  { id: 'p2', name: 'Casa Roja',  address: 'Calle 2', price: 200000, year: 2020 },
];

describe('PropertiesTable', () => {
  test('renderiza headers y filas con formateo de precio y links', () => {
    render(<PropertiesTable data={data as any} />);

    const table = screen.getByRole('table');

    // Headers
    const headers = within(table).getAllByRole('columnheader');
    expect(headers.map((h) => h.textContent?.trim())).toEqual(
      ['Name', 'Address', 'Price', 'Year', 'Action']
    );

    // Filas del cuerpo (omite la fila de header)
    const allRows = within(table).getAllByRole('row');
    const bodyRows = allRows.slice(1);
    expect(bodyRows).toHaveLength(2);

    const row1 = bodyRows[0];
    expect(within(row1).getByText(/casa verde/i)).toBeInTheDocument();
    expect(within(row1).getByText(/calle 1/i)).toBeInTheDocument();
    expect(within(row1).getByText(/\$100,000\.00/)).toBeInTheDocument();
    expect(within(row1).getByText('2010')).toBeInTheDocument();
    expect(within(row1).getByRole('link', { name: /view/i })).toHaveAttribute('href', '/properties/p1');

    const row2 = bodyRows[1];
    expect(within(row2).getByText(/\$200,000\.00/)).toBeInTheDocument();
    expect(within(row2).getByRole('link', { name: /view/i })).toHaveAttribute('href', '/properties/p2');
  });
});