import '@testing-library/jest-dom';
import { afterEach, vi } from 'vitest';
import { cleanup } from '@testing-library/react';

afterEach(() => cleanup());

// Mocks útiles para Next
vi.mock('next/link', () => ({
  default: (props: any) => <a {...props} />,
}));

// Si algún test usa <Image />, evita errores:
vi.mock('next/image', () => ({
  default: (props: any) => <img {...props} />,
}));
