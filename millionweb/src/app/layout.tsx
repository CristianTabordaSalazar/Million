import 'bootstrap/dist/css/bootstrap.min.css';
import { ReactQueryProvider } from '@/components/ReactQueryProvider';

export const metadata = { title: 'Properties', description: 'Property browser' };

export default function RootLayout({ children }: { children: React.ReactNode }) {
  return (
    <html lang="en">
      <body>
        <ReactQueryProvider>
          {children}
        </ReactQueryProvider>
      </body>
    </html>
  );
}
