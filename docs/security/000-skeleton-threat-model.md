# 000: Threat Model, Walking Skeleton (M1)

Status: Current · Date: 2026-11-08 · Scope: everything in M1 (API skeleton, local database, CI). No user data or features exist yet.

## What we're protecting

| Asset | Why it matters |
|---|---|
| Database credentials | Full read/write access to all future user data |
| Future API keys (Claude) | Direct cost to us; abuse could run up a bill |
| Source code integrity | A malicious change could ship to production via CI |
| Internal details (hosts, versions, stack traces) | Help an attacker map the system |

## Trust boundaries

```
Developer Mac ──push──► GitHub (public repo) ──► GitHub Actions runner
     │
     ├── API (localhost:5049)
     └── PostgreSQL container (127.0.0.1:5433)
```

## Threats and controls

| # | Threat (STRIDE) | Example | Control in place | Verified by |
|---|---|---|---|---|
| T1 | Information disclosure | DB password or API key committed to the public repo | `.env` git-ignored; `.env.example` holds placeholders only; connection string in .NET user-secrets (outside the repo); gitleaks scans full history in CI | `git check-ignore .env`; CI `secret-scan` job |
| T2 | Information disclosure | Local database reachable from other devices on the same Wi-Fi | Docker port bound to `127.0.0.1` only | `docker compose ps` shows `127.0.0.1:5433->5432` |
| T3 | Tampering | A dependency with a known vulnerability (CVE) is pulled in | CI fails on vulnerable NuGet packages, including transitive ones | CI `Fail on vulnerable packages` step |
| T4 | Information disclosure | Health endpoint leaks DB host, user or exception text | Health responses return only `Healthy`/`Unhealthy`; details go to logs | Test `Ready_does_not_leak_error_details` |
| T5 | Elevation of privilege | Compromised workflow uses its token to modify the repo | Workflow token limited to `contents: read` | `.github/workflows/ci.yml` `permissions` block |
| T6 | Denial of service | Slow/hung database makes health checks hang | Readiness check has a 3s timeout; liveness never touches the DB | Code review: `Program.cs`; tests for DB-down |

## Accepted risks (for now)

| Risk | Why accepted | Revisit |
|---|---|---|
| Weak local DB password (`change-me-local-only`) | Local-only, bound to 127.0.0.1, no real data | M6: production uses Azure Key Vault + strong generated credentials |
| GitHub Actions pinned by version tag, not commit SHA | A compromised action tag could run in CI; low impact while the token is read-only and no secrets exist in CI | Before adding any CI secrets (M6): pin actions to full commit SHAs |
| No HTTPS locally | Development only | M6: HTTPS enforced by Azure |

## Next threat model

M2 (auth, households): broken object-level authorization (IDOR), credential stuffing, session/token handling, tenant data isolation.
