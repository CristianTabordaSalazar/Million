import { z } from "zod";
import { OwnerSchema } from "./owner.schema";
import { PropertyTraceSchema } from "./propertyTrace.schema";

export const PropertyDetailSchema = z.object({
  id: z.string().uuid(),
  name: z.string(),
  address: z.string(),
  price: z.number(),
  codeInternal: z.string(),
  year: z.number().int(),
  owner: OwnerSchema,
  firstImageUrl: z.string().url().nullable().optional(),
  traces: z.array(PropertyTraceSchema),
});

export type PropertyDetailResponse = z.infer<typeof PropertyDetailSchema>;