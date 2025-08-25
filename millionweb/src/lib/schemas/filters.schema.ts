import { z } from 'zod';

export const FiltersSchema = z.object({
  name: z.string().trim().optional(),
  address: z.string().trim().optional(),
  minPrice: z.union([z.coerce.number(), z.nan()]).optional(),
  maxPrice: z.union([z.coerce.number(), z.nan()]).optional(),
  page: z.coerce.number().min(1).default(1),
  pageSize: z.coerce.number().min(5).max(100).default(10),
});

export type Filters = z.output<typeof FiltersSchema>;
export type FiltersIn = z.input<typeof FiltersSchema>;
