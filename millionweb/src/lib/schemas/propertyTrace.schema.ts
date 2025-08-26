import { z } from "zod";

export const PropertyTraceSchema = z.object({
  id: z.string().uuid(),
  dateSale: z.string(),
  name: z.string(),
  value: z.number(),
  tax: z.number(),
});

export type PropertyTraceResponse = z.infer<typeof PropertyTraceSchema>;