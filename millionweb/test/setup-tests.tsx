import '@testing-library/jest-dom';
import { afterEach, vi } from 'vitest';
import { cleanup } from '@testing-library/react';

afterEach(() => cleanup());

vi.mock('next/link', () => ({
  default: (props: any) => <a {...props} />,
}));

vi.mock('next/image', () => ({
  default: (props: any) => <img {...props} />,
}));
