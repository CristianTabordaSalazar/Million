import { z } from 'zod';

export const PropertySchema = z.object({
  id: z.string(),
  name: z.string(),
  address: z.string(),
  price: z.number(),
  codeInternal: z.string().optional(),
  year: z.number().optional(),
});

export type Property = z.infer<typeof PropertySchema>;
