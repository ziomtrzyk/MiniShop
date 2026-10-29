# MiniShop – learning roadmap

A smaller, self-built shop inspired by [dotnet/eShop](https://github.com/dotnet/eShop).
The goal is to learn the concepts behind eShop by building them step by step,
not to copy it.

Local eShop clone for reference: `../eShop` (checked at commit `913acea`, 2026-10-15).

## Rules

- Every stage ends with something that **runs** and can be shown.
- New concepts per stage are limited to a few – everything else reuses what is already known.
- After each stage: compare with the matching part of eShop (see "Compare with").
- Mark progress here (`[x]`) so every new session knows where we stopped.

## Target architecture

```
                    ┌──────────────┐
                    │  WebApp      │  Blazor (stage 8)
                    └──────┬───────┘
          HTTP / gRPC      │
   ┌──────────────┬────────┴─────┬───────────────┐
   ▼              ▼              ▼               ▼
Catalog.API   Basket.API    Ordering.API    Identity.API
(PostgreSQL)  (Redis)       (PostgreSQL,    (stage 7)
                             DDD + CQRS)
   │              │              │
   └──────────────┴──── RabbitMQ ┴──────► OrderProcessor (worker)
                    (integration events)

All services are started and wired together by MiniShop.AppHost (Aspire).
```

---

## Stage 0 – Setup

New concepts: solution with many projects, central package management.

- [x] Install Docker Desktop and check `docker run hello-world`
- [x] `git init`, `.gitignore` (`dotnet new gitignore`)
- [x] Solution file `MiniShop.slnx`
- [x] `Directory.Build.props` (shared `TargetFramework`, `Nullable`, `ImplicitUsings`)
- [x] `Directory.Packages.props` (all package versions in one place)
- [x] Folders: `src/`, `tests/`

Done when: `dotnet build` builds an empty solution.
Compare with: `eShop.slnx`, `Directory.Build.props`, `Directory.Packages.props`.

## Stage 1 – Catalog.API

New concepts: Minimal APIs with `MapGroup` / `TypedResults`, PostgreSQL, pagination, `CancellationToken` everywhere.

- [x] Entities: `CatalogItem`, `CatalogBrand`, `CatalogType`
- [x] EF Core with Npgsql, PostgreSQL in a Docker container
- [x] Seed data on startup
- [X] Endpoints: list (paginated), get by id, filter by brand/type, create, update, delete
- [x] `PaginatedItems<T>` response
- [x] OpenAPI + Scalar

Done when: products can be browsed page by page in Scalar.
Compare with: `src/Catalog.API/Apis/CatalogApi.cs`, `src/Catalog.API/Model/`.

## Stage 2 – .NET Aspire

New concepts: AppHost, ServiceDefaults, service discovery, health checks, OpenTelemetry, dashboard.

- [x] `MiniShop.AppHost` – starts PostgreSQL and Catalog.API
- [x] `MiniShop.ServiceDefaults` – health checks, telemetry, resilience
- [x] Connection string comes from Aspire, not `appsettings.json`
- [x] Remove the manual Docker command for PostgreSQL

Done when: one `dotnet run` on AppHost starts everything; logs and traces are visible in the dashboard.
Compare with: `src/eShop.AppHost/`, `src/eShop.ServiceDefaults/`.

## Stage 3 – Basket.API

New concepts: Redis, gRPC, service-to-service calls.

- [x] Basket stored in Redis (key = user id)
- [x] Version A: REST endpoints (get / update / delete basket)
- [x] Version B: rewrite as gRPC service (`.proto` file)
- [x] Redis added in AppHost
- [x] Basket.API fetches product data from Catalog.API

Done when: a basket can be saved and read back through gRPC.
Compare with: `src/Basket.API/Proto/`, `src/Basket.API/Grpc/`, `src/Basket.API/Repositories/`.

## Stage 4 – Ordering.API (DDD + CQRS)

New concepts: Clean Architecture split, aggregates, value objects, domain events, MediatR, FluentValidation, pipeline behaviors.

- [ ] Projects: `Ordering.Domain`, `Ordering.Infrastructure`, `Ordering.API`
- [ ] Aggregate `Order` with `OrderItem`, value object `Address`
- [ ] Business methods instead of public setters (`SetPaidStatus()`, `Cancel()`)
- [ ] Repository + Unit of Work
- [ ] Commands + handlers (`CreateOrderCommand`, `CancelOrderCommand`)
- [ ] Queries (`GetOrdersQuery` / `OrderQueries`)
- [ ] Behaviors: logging, validation, transaction
- [ ] Domain events dispatched before `SaveChanges`

Done when: an order can be created and cancelled, and invalid state changes throw domain exceptions.
Compare with: `src/Ordering.Domain/AggregatesModel/`, `src/Ordering.API/Application/Commands/`,
`Queries/`, `Behaviors/`, `Validations/`, `DomainEventHandlers/`.

## Stage 5 – Messaging with RabbitMQ

New concepts: integration events vs domain events, event bus abstraction, eventual consistency, idempotency, Outbox pattern.

- [ ] Simple `IEventBus` abstraction + RabbitMQ implementation
- [ ] `OrderStartedIntegrationEvent` → Basket.API clears the basket
- [ ] `ProductPriceChangedIntegrationEvent` from Catalog.API → Basket.API updates prices
- [ ] Outbox table: event saved in the same transaction as the data, published afterwards
- [ ] Handlers safe to run twice (idempotent)

Done when: placing an order empties the basket without a direct call between services.
Compare with: `src/EventBus/`, `src/EventBusRabbitMQ/`, `src/IntegrationEventLogEF/`,
`src/Ordering.API/Application/IntegrationEvents/`.

## Stage 6 – Background workers

New concepts: `BackgroundService`, multi-step order process across services.

- [ ] `OrderProcessor` – moves orders from "submitted" to "awaiting validation" after a grace period
- [ ] Catalog.API checks stock and answers with an event
- [ ] `PaymentProcessor` – fake payment, publishes success / failure
- [ ] Order status flow: Submitted → AwaitingValidation → StockConfirmed → Paid → Shipped

Done when: an order goes through all statuses by itself, visible in the Aspire traces.
Compare with: `src/OrderProcessor/`, `src/PaymentProcessor/`.

## Stage 7 – Identity

New concepts: OAuth 2.0, OpenID Connect, access tokens between services.

- [ ] Theory: authorization code + PKCE, scopes, `id_token` vs `access_token`
- [ ] `Identity.API` with ASP.NET Core Identity + an OIDC server
- [ ] Basket.API and Ordering.API only validate tokens
- [ ] User id taken from token claims

Done when: basket and orders are tied to the logged-in user.
Compare with: `src/Identity.API/`.

## Stage 8 – WebApp (Blazor)

New concepts: Blazor components, render modes, OIDC login in Blazor, typed HTTP / gRPC clients.

- [ ] Catalog page with pagination
- [ ] Basket page
- [ ] Checkout + order history
- [ ] Login / logout

Done when: a full purchase can be made in the browser.
Compare with: `src/WebApp/`, `src/WebAppComponents/`.

## Stage 9 – Tests (start already in stage 4)

New concepts: unit tests for the domain, integration tests, Testcontainers, Aspire testing.

- [ ] `tests/Ordering.UnitTests` – aggregate rules
- [ ] `tests/Catalog.FunctionalTests` – API with a real database
- [ ] One end-to-end test through AppHost

Compare with: `tests/`.

## Optional extras

- [ ] API gateway / BFF with YARP
- [ ] Webhooks
- [ ] Semantic search in catalog (AI embeddings)

---

## Progress log

| Date | Stage | Notes |
|------|-------|-------|
|      |       |       |
