import { z } from "zod";

export const OwnerSchema = z.object({
  id: z.string().uuid().or(z.literal("00000000-0000-0000-0000-000000000000")),
  name: z.string(),
  address: z.string(),
  photo: z.string().url().optional().or(z.literal("")).default(""),
  dateOfBirth: z.string(),
});

export type OwnerResponse = z.infer<typeof OwnerSchema>;