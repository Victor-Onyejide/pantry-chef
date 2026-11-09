# Pantry Chef

Tell it what's in your kitchen; get dishes you can cook now — never containing anything your household is allergic to.

Stack: React + TypeScript · .NET · PostgreSQL · Claude API · Docker · Azure

## Layout

```
backend/    .NET API, worker, tests
frontend/   React + TypeScript app
evals/      evaluation set and runner for dish suggestions
docs/design design documents
```

## Milestones

- [x] M0 — Design: [001 Dish Suggestions](docs/design/001-dish-suggestions.md)
- [ ] M1 — Skeleton: .NET solution, PostgreSQL in Docker Compose, health endpoint, CI on GitHub Actions
- [ ] M2 — Auth, households, members, pantry, dietary restrictions
- [ ] M3 — Suggestion pipeline: async job, model call, safety layers, idempotency
- [ ] M4 — Eval harness: 30 cases, metrics report, model comparison
- [ ] M5 — React UI: pantry, suggestions with polling, ratings
- [ ] M6 — Deploy to Azure; logging, tracing, cost tracking
