'use client';
import { useQuery, keepPreviousData } from '@tanstack/react-query';
import { fetchProperties, fetchPropertyDetail } from '@/lib/api';
import { Filters } from '@/lib/schemas';

export function useProperties(filters: Filters) {
  return useQuery({
    queryKey: ['properties', filters],
    queryFn: () => fetchProperties(filters),
    placeholderData: keepPreviousData,
  });
}

import { fetchPropertyById } from '@/lib/api';

export function useProperty(id: string) {
  return useQuery({
    queryKey: ['property', id],
    queryFn: () => fetchPropertyById(id),
    enabled: !!id,
  });
}

export function usePropertyDetail(id?: string) {
  return useQuery({
    queryKey: ["property-detail", id],
    queryFn: () => fetchPropertyDetail(id as string),
    enabled: !!id,
    staleTime: 60_000,
  });
}