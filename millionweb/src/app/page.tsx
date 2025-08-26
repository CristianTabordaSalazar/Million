'use client';
import Link from 'next/link';

const DEVELOPER_NAME = 'Cristian Taborda';

export default function WelcomePage() {
  return (
    <div className="container py-5">
      <div className="row justify-content-center">
        <div className="col-lg-10">
          <div className="card shadow border-0">
            <div className="card-body p-5">
              <div className="text-center mb-4">
                <span className="badge text-bg-primary px-3 py-2">
                  Technical Test – Sr Developer Full‑Stack
                </span>
              </div>

              <h1 className="display-5 fw-bold text-center mb-3">
                Welcome to <span className="text-primary">Million</span> Property System
              </h1>

              <div className="d-flex justify-content-center mb-4">
                <div className="text-center">
                  <div className="fs-6 text-uppercase text-muted">Presented by</div>
                  <div className="fs-4 fw-semibold">{DEVELOPER_NAME}</div>
                </div>
              </div>

              <hr className="my-4" />

              <div className="row g-4">
                <div className="col-md-12">
                  <div className="p-4 bg-light rounded h-100">
                    <h5 className="fw-semibold mb-2">Stack</h5>
                    <ul className="mb-0">
                      <li>Frontend: Next.js + React </li>
                      <li>Backend: ASP.NET Core 9.0</li>
                      <li>Database: MongoDB</li>
                      <li>Clean Architecture</li>
                    </ul>
                  </div>
                </div>
              </div>

              <div className="d-flex flex-wrap gap-3 justify-content-center mt-5">
                <Link href="/properties" className="btn btn-primary btn-lg">
                  Browse Properties
                </Link>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
