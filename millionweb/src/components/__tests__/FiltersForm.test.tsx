import { render, screen, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import FiltersForm from '../FiltersForm';
import { vi, describe, test, expect } from 'vitest';

vi.mock('@/lib/schemas', async () => {
  const z = await import('zod');
  const FiltersSchema = z.object({
    name: z.string().optional(),
    address: z.string().optional(),
    minPrice: z.number().int().positive().optional(),
    maxPrice: z.number().int().positive().optional(),
    page: z.number().int().positive(),
    pageSize: z.number().int().positive(),
  });
  return { FiltersSchema };
});

describe('FiltersForm', () => {
  test('Triggers onChange with debounce', async () => {
    const user = userEvent.setup({ delay: null });
    const onChange = vi.fn();

    render(<FiltersForm initial={{ pageSize: 10 }} onChange={onChange} />);

    await user.type(screen.getByLabelText(/name/i), 'Blue House');
    await user.type(screen.getByLabelText(/address/i), '123 Street');
    await user.type(screen.getByLabelText(/min price/i), '1000');
    await user.type(screen.getByLabelText(/max price/i), '5000');

    await new Promise((r) => setTimeout(r, 450));

    await waitFor(() => {
      expect(onChange).toHaveBeenCalled();
    });

    const last = onChange.mock.calls.at(-1)![0];
    expect(last).toMatchObject({
      name: 'Blue House',
      address: '123 Street',
      minPrice: 1000,
      maxPrice: 5000,
      page: 1,
      pageSize: 10,
    });
  });

  test('Submit sets page=1', async () => {
    const user = userEvent.setup({ delay: null });
    const onChange = vi.fn();

    render(<FiltersForm initial={{ pageSize: 20 }} onChange={onChange} />);

    await user.type(screen.getByLabelText(/name/i), 'Centro');
    await user.click(screen.getByRole('button', { name: /search/i }));

    await waitFor(() => expect(onChange).toHaveBeenCalled());

    const payload = onChange.mock.calls.at(-1)![0];
    expect(payload).toMatchObject({ name: 'Centro', page: 1, pageSize: 20 });
  });

  test('Clear resets filters', async () => {
    const user = userEvent.setup({ delay: null });
    const onChange = vi.fn();

    render(<FiltersForm initial={{ pageSize: 10 }} onChange={onChange} />);

    await user.type(screen.getByLabelText(/name/i), 'X');
    await user.type(screen.getByLabelText(/address/i), 'Y');

    await user.click(screen.getByRole('button', { name: /clear/i }));

    await waitFor(() => expect(onChange).toHaveBeenCalled());

    const payload = onChange.mock.calls.at(-1)![0];
    expect(payload).toEqual({ page: 1, pageSize: 10 });

    expect((screen.getByLabelText(/name/i) as HTMLInputElement).value).toBe('');
    expect((screen.getByLabelText(/address/i) as HTMLInputElement).value).toBe('');
  });
});
