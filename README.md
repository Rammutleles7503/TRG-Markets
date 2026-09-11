# TRG Markets

![TRG Markets — Trade. Grow. Succeed.](assets/branding/TRG-Markets-Logo.png)

TRG Markets is an ASP.NET Core Clean Architecture platform for trading-account,
trade, notification, system-alert and market-holiday management.

## Current milestone: Profit Light Phase 1

- Deterministic score-to-light mapping.
- Fail-closed stale and incomplete-data handling.
- Safety authority and emergency override precedence.
- Non-positive expected-value Green prevention.
- Persisted assessment audit history.
- `POST /api/profit-light/pre-trade`.
- `GET /api/profit-light/ea/{eaId}/latest`.
- Boundary and hard-override tests in `TRG-Markets.Tests`.

Profit Light is decision support, not a profit guarantee. Capital protection always has final authority.

## Solution projects

- `TRG-Markets.Domain` — domain entities.
- `TRG-Markets.Application` — service contracts and application rules.
- `TRG-Markets . Persistence` — Entity Framework Core persistence.
- `TRG-Markets . Infrastructure` — service implementations and integrations.
- `TRG-Markets.API` — ASP.NET Core REST API and composition root.

## Build

Prerequisite: .NET 10 SDK.

```powershell
dotnet restore .\TRG-Markets.slnx
dotnet build .\TRG-Markets.slnx --configuration Release --no-restore
dotnet test .\TRG-Markets.slnx --configuration Release --no-build
```

The API uses SQL Server LocalDB through the development connection string, so
run database-backed endpoints from Windows with LocalDB available or provide an
environment-specific `ConnectionStrings__DefaultConnection` value.

## Architecture documents

- [EA Profit Light™ Architecture](docs/architecture/TRG-MARKETS_EA_Profit_Light_Architecture.md)

EA Profit Light™ is a profit-first decision-support and live-trade monitoring
component. It never guarantees profit and cannot override TRG Entry Authority™,
the Capital Execution Guardian, hard risk limits, CyberShield™ or Emergency Stop.

## GitHub readiness

The repository excludes Visual Studio caches, build outputs, user settings and
local environment files. GitHub Actions restores, builds and tests the solution
on each push and pull request.
