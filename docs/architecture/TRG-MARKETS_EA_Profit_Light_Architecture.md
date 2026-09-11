# TRG-MARKETS EA Profit Light™ Architecture

## 1. Purpose

TRG-MARKETS EA Profit Light™ is a real-time decision-support indicator that shows the **estimated probability that an EA's next approved trade, or an existing open trade, will finish profitably**.

It is not a profit guarantee. It must display `Probability estimate — not guaranteed` and must never override capital protection, the TRG Entry Authority™, TRG Global Risk Score™, the Capital Execution Guardian, or an Emergency Stop.

## 2. Dashboard Light

| Light | Score | Meaning | System action |
| --- | ---: | --- | --- |
| Bright Green | 85–100 | Very strong profit conditions | Eligible for normal approved risk |
| Green | 75–84 | Favourable profit conditions | Eligible after all safety checks |
| Blue | 65–74 | Developing or stable opportunity | Watch; reduced risk or wait for confirmation |
| Amber | 50–64 | Uncertain or mixed evidence | Delay new entry |
| Orange | 35–49 | Unfavourable and deteriorating | Block additions; protect open profit |
| Red | 1–34 | High loss/failure risk | Reject entry; reduce or close only under policy |
| Grey | No reliable score | Data is valid but evidence or sample size is insufficient | Abstain; wait for more evidence and prohibit new entry |
| Black | 0 or safety override | Emergency, invalid data, disconnection or hard risk breach | Suspend/quarantine EA and alert CEO |

The light must never show green when Entry Authority returns Reject, the Guardian is in Preservation/Emergency mode, data is stale, a hard risk limit is breached, or expected value after all trading costs is zero or negative.

### 2.1 Profit-First Dashboard Presentation

The dashboard must make profit opportunity the primary visual focus without hiding capital-protection information:

- Display one large central **Profit Light** showing the current state, score or calibrated probability range, direction arrow and freshness timer.
- When conditions are favourable, Green or Bright Green occupies the main visual area and clearly shows the estimated profit opportunity, expected value after costs and recommended action.
- Show Profit Continuation, current net profit, protected profit, profit high-watermark and remaining permitted giveback directly below the main light for an open trade.
- Display loss risk, uncertainty, execution health and safety overrides as smaller secondary warning indicators beneath or beside the main light.
- Secondary warnings must remain immediately visible and may never be hidden, disabled or coloured green when their underlying condition is unsafe.
- Amber, Orange, Red, Grey or Black automatically replaces the primary green display whenever the governed final state requires caution, rejection, abstention or emergency action.
- The interface must use plain-language actions such as `Enter`, `Hold`, `Protect Profit`, `Partial Close`, `Move to Break-Even`, `Trail`, `Close`, `Wait` or `Unavailable`.
- A profit-focused presentation must never suppress a downgrade, delay an emergency state, imply guaranteed profit or override Entry Authority, Global Risk Score, the Capital Execution Guardian, CyberShield™, Execution Integrity or Emergency Stop.

The design principle is: **profit opportunity is prominent; loss and safety intelligence remain smaller but always active, visible and authoritative.**

## 3. Profit Likelihood Score

| Component | Weight |
| --- | ---: |
| Signal and market-structure quality | 25% |
| Market-regime fit for the EA | 15% |
| EA validated historical performance in similar conditions | 15% |
| Risk, margin, drawdown and portfolio capacity | 15% |
| Spread, slippage, latency and broker execution quality | 10% |
| Trend, momentum and volatility agreement | 10% |
| Session, news and liquidity conditions | 5% |
| Correlation and crowding safety | 5% |

`Raw Score = weighted sum of component scores (0–100)`

The displayed percentage must be calibrated against verified out-of-sample and live results. Until enough reliable samples exist, the UI labels the value `Confidence score` rather than `Profit probability`.

## 4. Two Operating Views

### Pre-Trade Profit Light

Returns light colour, score, confidence, freshness, reasons, recommended action, reward-to-risk and expected value after costs before Entry Authority permits an order.

### Live-Trade Profit Continuation Light

Recalculates while a position is open. It can recommend Hold, Protect, Partial Close, Move to Break-Even, Trail, Close or Emergency Close. Actions remain governed by approved risk policies.

## 5. Supporting Mini-Indicators

- Green bar — Profit Continuation.
- Red bar — Failure Risk.
- Blue bar — Stability.
- Direction arrow — rising, stable or falling.
- Freshness timer — seconds since the last complete calculation.

### 5.1 Chart Analysis Engine

The Profit Light must analyze the chart before a proposed entry and continuously while a trade is open. It must combine chart evidence with account, broker, execution and risk evidence rather than treating an indicator signal as proof of profit.

The chart analysis includes:

- Price direction and current trend.
- Market structure: higher highs, higher lows, lower highs, lower lows, breakouts, structure shifts and confirmed reversals.
- Support, resistance, interactive supply and demand zones.
- Momentum, trend strength and volatility conditions.
- Liquidity sweeps, false breakouts, rejection behaviour and abnormal price movement.
- Multi-timeframe agreement across the EA's approved timeframes, including M5, M15, M30 and H1 where applicable.
- The EA's proposed Buy or Sell direction and the quality of its underlying signal.
- Spread, commission, estimated slippage, latency and broker execution quality.
- Trading session, economic-news restrictions and available liquidity.
- Open positions, account margin, drawdown and remaining portfolio-risk capacity.
- Correlation, duplicated exposure and crowding across all active EAs.
- The EA's validated performance in comparable historical market regimes.

The engine returns a 0–100 score, light colour, confidence level, rising/stable/falling trend, Profit Continuation, Failure Risk, Stability, expected value after costs, reward-to-risk and a recommended action. Recommended actions are `Wait`, `Enter`, `Hold`, `Protect`, `Partial Close`, `Move to Break-Even`, `Trail`, `Close` or `Emergency Close`.

A strong aligned trend, confirmed structure and momentum, acceptable trading costs and sufficient risk capacity may support a Green light. A confirmed reversal, major-news restriction, excessive spread, stale data, execution failure or hard risk breach must cap or override the result to Amber, Orange, Red or Black according to policy.

Chart analysis is probabilistic and must never promise profit. No chart-based result may override the TRG Entry Authority™, TRG Global Risk Score™, Capital Execution Guardian, CyberShield™, Execution Integrity controls or Emergency Stop.

## 6. Safety Overrides

The light is capped or overridden for excessive spread/slippage, degraded broker or bridge health, news restrictions, risk-limit pressure, invalid symbol/session/timeframe, insufficient comparable data, confirmed reversal, or CyberShield™ risk. Every override must show its exact reason.

## 7. Learning and Validation

Store every prediction, input snapshot, version, decision, execution cost and result. Monitor calibration, net profit after costs, drawdown, false-green/false-red rates and drift separately for each EA and regime. Promotion requires walk-forward, out-of-sample, demo and controlled-live validation.

## 8. TRG-MARKETS Integration

`EA signal → Opportunity Radar → Profit Light → Entry Authority → Global Risk Score → Capital Execution Guardian → MT4/MT5 bridge`

Results appear in EA Management, Trading Accounts, AI Control Tower, CEO Dashboard, Emergency Command Center, Notifications & Alert Center, Trade Replay Center and Audit & Logs.

Initial EAs: GrayBird PRO (710001), Gold Scalper PRO (710002), Cobra Sniper PRO (710003), Dark Dione (710004), and ShootingStar PRO (710005).

## 9. Minimum Data Contract

```text
eaId, magicNumber, accountId, broker, symbol, timeframe, direction
score, displayLabel, lightState, trend, confidence, dataFreshness
probabilityLowerBound, probabilityUpperBound, comparableSampleSize
predictionHorizon, outcomeDefinition, abstentionReason
profitContinuation, failureRisk, stabilityScore
expectedValue, rewardRisk, estimatedCosts
topPositiveReasons[], topNegativeReasons[], overrideReasons[]
guardianDecision, entryAuthorityDecision, recommendedAction
modelVersion, calculatedAtUtc, expiresAtUtc
```

## 10. Governance Rule

**A green light means conditions are favourable—not that profit is certain. Capital protection always has final authority.**

## 11. Clean Architecture Implementation

### Domain

- `ProfitLightAssessment`: immutable assessment result and audit identity.
- `ProfitLightComponentScore`: component name, raw score, weight and contribution.
- `ProfitLightOverride`: override type, severity, reason and source engine.
- Enums: `ProfitLightState`, `ProfitLightTrend`, `ProfitLightAction`, `ConfidenceLevel`.
- Domain rules enforce score range, expiry, mandatory reasons and safety precedence.

### Application

- `IProfitLightService.CalculatePreTradeAsync(request)`
- `IProfitLightService.CalculateLiveTradeAsync(request)`
- `IProfitLightQueryService.GetLatestAsync(...)`
- `IProfitLightCalibrationService.RecordOutcomeAsync(...)`
- DTOs, validators, authorization policies and mapping live here.

### Infrastructure

- Market-data, broker-health, news, execution-quality, risk and MT4/MT5 bridge adapters.
- Versioned deterministic scoring engine for V1.
- Optional ML calibration provider may be introduced only after approved validation.

### Persistence

- EF Core configurations, repositories, indexed assessment history and outcome records.
- Append-only audit data; corrections create a new record rather than rewriting evidence.

### API

- Authenticated REST endpoints, role authorization, rate limits and correlation IDs.
- SignalR/event stream may publish safe dashboard updates without exposing credentials.

## 12. Core Entity Fields

`ProfitLightAssessment` must include: `Id`, `AssessmentType`, `EAId`, `MagicNumber`, `TradingAccountId`, `TradeId`, `Broker`, `Symbol`, `Timeframe`, `Direction`, `RawScore`, `CalibratedScore`, `LightState`, `Trend`, `Confidence`, `RecommendedAction`, `ProfitContinuation`, `FailureRisk`, `StabilityScore`, `ExpectedValue`, `RewardRisk`, `EstimatedCosts`, `EntryAuthorityDecision`, `GuardianDecision`, `ModelVersion`, `InputSnapshotHash`, `CalculatedAtUtc`, `ExpiresAtUtc`, `CreatedAtUtc`.

Reasons, component scores and overrides are stored as child records so every displayed decision is explainable and auditable.

## 13. Deterministic Calculation and Precedence

1. Validate identity, account, EA, symbol, timeframe and timestamps.
2. Confirm required feeds are available and fresh.
3. Normalize each component to 0–100.
4. Apply versioned weights and calculate the raw score.
5. Calculate expected value after spread, commission, swaps and estimated slippage.
6. Apply confidence/data sufficiency caps.
7. Apply soft restrictions and reduced-risk rules.
8. Apply hard overrides last; hard overrides always win.
9. Generate plain-language reasons and expiry.
10. Persist the full assessment before publishing it.

Safety precedence:

`Emergency Stop → CyberShield/Execution Integrity → Hard Risk Limits → Guardian → Entry Authority → Profit Light recommendation`

The service fails closed: missing mandatory data, invalid values or timeout produces Black/Unavailable and no new entry.

## 14. API Endpoints

| Method | Route | Purpose |
| --- | --- | --- |
| POST | `/api/profit-light/pre-trade` | Calculate and persist a proposed-entry assessment |
| POST | `/api/profit-light/live-trade` | Calculate and persist an open-position assessment |
| GET | `/api/profit-light/ea/{eaId}/latest` | Latest valid EA result |
| GET | `/api/profit-light/trade/{tradeId}/history` | Trade assessment timeline |
| GET | `/api/profit-light/account/{accountId}/summary` | Account-wide lights and overrides |
| POST | `/api/profit-light/{id}/outcome` | Record verified outcome for calibration |
| GET | `/api/profit-light/health` | Dependencies, freshness and model version |

Only internal trusted services may request calculations or submit outcomes. Dashboard users receive read-only results according to role.

## 15. Notifications and Automatic Actions

The Notifications & Alert Center receives events when:

- a light moves to Orange, Red or Black;
- a score drops rapidly while a trade is open;
- a green assessment is overridden by a safety authority;
- data becomes stale or a dependency fails;
- false-green rate or calibration drift crosses its limit;
- an EA is suspended or quarantined.

Profit Light never directly opens a trade. Protective actions require policy authorization, idempotency keys, complete audit logs and confirmation from the Capital Execution Guardian.

## 16. Security, Reliability and Observability

- Encrypt data in transit and at rest; never store broker passwords in assessment data.
- Apply least-privilege roles and CyberShield™ controls.
- Use UTC, correlation IDs, idempotency and bounded retries.
- Reject replayed, duplicated, stale or tampered input snapshots.
- Record calculation latency, dependency latency, timeout rate, override rate and light distribution.
- A health failure changes the light to Black/Unavailable; the previous green result must not remain active after expiry.

## 17. TRG MQL5 Build & Test Authority™

All EA-side Profit Light adapters must pass the mandatory release gate:

1. Clean MT5/MT4 compilation with zero errors and approved warning threshold.
2. MQL5 unit/component tests for normalization, boundaries, expiry and override precedence.
3. Strategy Tester runs across approved symbols, timeframes and adverse conditions.
4. Bridge contract, disconnect, reconnect, duplicate-message and stale-data tests.
5. Demo forward test and controlled-live approval.
6. Signed/versioned build, checksum, rollback package and release record.

No EA adapter may enter production when a test fails, evidence is missing or its architecture/model version does not match the platform.

## 18. Phased Delivery and Acceptance

### Phase 1 — Foundation

Implement enums, entities, EF Core migration, deterministic scoring, pre-trade endpoint and audit history. Acceptance: build succeeds; boundary and hard-override tests pass.

### Phase 2 — Live Protection

Add live-trade recalculation, SignalR updates, Notifications integration and Capital Execution Guardian recommendations. Acceptance: stale data fails closed and every state transition is auditable.

### Phase 3 — EA Bridge

Integrate the five initial EA magic numbers through versioned MT4/MT5 contracts. Acceptance: disconnect/replay/idempotency and MQL5 release gates pass.

### Phase 4 — Calibration and Controlled Live

Add outcome reconciliation, per-EA/regime calibration, drift monitoring and CEO reporting. Acceptance: approved sample thresholds and false-green limits are achieved before the UI may say `Profit probability`.

## 19. First Coding Milestone

The first compiling commit should contain only:

- four enums and the `ProfitLightAssessment` domain entity;
- request/result DTOs and validators;
- `IProfitLightService` plus deterministic V1 implementation;
- EF Core configuration and migration;
- `POST /api/profit-light/pre-trade` and `GET /api/profit-light/ea/{eaId}/latest`;
- unit tests for score bands, expiry, missing data and safety override precedence.

This keeps TRG-MARKETS aligned with the rule: **one compiling, testable commit at a time**.

## 20. MQL5 Build-Time Unit-Testing Framework

TRG-MARKETS will maintain a reusable MQL5 test framework for Profit Light and every supported EA. Testable trading logic must be separated from `OnTick()`, broker I/O and chart controls so that scoring, risk and safety rules can run deterministically with fixed inputs.

### Test Package Structure

```text
MQL5/
  Include/TRG/Test/          assertions, runner, fixtures and mocks
  Include/TRG/ProfitLight/   pure scoring, bands, freshness and override rules
  Scripts/TRG/Tests/         executable unit and contract test suites
  Experts/TRG/Adapters/      thin production EA adapters
  Files/TRG/TestResults/     machine-readable test evidence
```

### Mandatory Test Suites

- Score normalization, weighting and exact colour-band boundaries.
- Confidence caps, insufficient-data handling and calibrated-label selection.
- Freshness, expiry, clock skew and stale-data fail-closed behaviour.
- Emergency Stop, CyberShield™, hard-risk, Guardian and Entry Authority precedence.
- Expected-value calculations including spread, commission, swap and slippage.
- Magic-number, symbol, timeframe, direction and account validation.
- Live-trade transitions: Hold, Protect, Partial Close, Break-Even, Trail and Close.
- Duplicate-event, reconnect, replay, idempotency and bridge-schema compatibility.
- Invalid numeric inputs, zero division, extreme values and unavailable dependencies.
- Regression fixtures for every previously confirmed false-green or safety incident.

### Assertion and Runner Contract

The framework must provide `AssertTrue`, `AssertFalse`, `AssertEqual`, `AssertNear`, `AssertBetween`, `AssertState`, `AssertAction` and `AssertFailsClosed`. Each test records suite, test name, EA/version, inputs, expected result, actual result, duration and pass/fail status.

The runner returns a non-success release status when any mandatory test fails. Results are written as JSON or JUnit-compatible XML so the TRG-MARKETS build pipeline can archive, compare and display them in Audit & Logs and the CEO Approval Center.

### Automated Release Pipeline

1. Compile shared MQL5 libraries and the target EA with the approved MetaEditor build.
2. Fail on compilation errors, prohibited warnings or an unapproved dependency.
3. Execute deterministic unit and component suites with a fixed random seed and UTC fixtures.
4. Run Strategy Tester scenarios for XAUUSD and NAS100 across approved timeframes, spreads, volatility regimes and disconnections.
5. Validate the Profit Light bridge contract against the matching platform schema/model version.
6. Generate a signed evidence manifest containing source commit, build number, compiler version, checksums and test results.
7. Promote only when every mandatory gate passes and the required approval is recorded.

### Release Blocking Rules

Production deployment is blocked when coverage for a safety-critical rule is missing, a boundary test fails, expected and deployed model versions differ, results are stale or unsigned, or a previously fixed regression returns. A failed build cannot be manually relabelled as passed; an authorized exception must be time-limited, fully audited and may never bypass Emergency Stop, hard-risk or fail-closed tests.

### Initial Profit Light Acceptance Pack

The first suite must prove that scores `0`, `1`, `34`, `35`, `49`, `50`, `64`, `65`, `74`, `75`, `84`, `85` and `100` map to the correct light; insufficient but valid evidence returns Grey/Abstain; expired or incomplete inputs return Black/Unavailable; a favourable raw score cannot defeat a Reject, non-positive net expected value or emergency decision; and identical inputs under the same version always return the same result.

## 21. Reliability, Abstention and Calibration Controls

### 21.1 Uncertainty Range and Evidence Strength

The dashboard must not present false precision. A calibrated result should display a probability interval, such as `68–76%`, together with confidence level, comparable sample size and data freshness. The interval must widen when comparable observations are limited, market conditions are unstable or model disagreement increases. Until minimum evidence thresholds are achieved, the result remains a `Confidence score` and may not be advertised as a profit probability.

### 21.2 Grey Abstention State

Grey means the required feeds are valid and operational, but the system does not have enough reliable evidence to issue a directional assessment. Grey is distinct from Black: Grey means `Abstain/Wait`, while Black means `Unavailable/Emergency`. Grey prohibits new entry and records an exact abstention reason such as insufficient comparable samples, conflicting models, unstable regime classification or an excessively wide uncertainty interval.

### 21.3 Net Expected-Value Gate

A Bright Green or Green state is forbidden when net expected value is zero or negative after spread, commission, swap, estimated slippage, financing and other applicable broker costs. The gate must use conservative cost estimates and stress them for fast markets. If the underlying score is favourable but the net expected-value gate fails, the light is capped at Amber and Entry Authority receives `Wait/Reject — costs exceed edge`.

### 21.4 Prediction Horizon and Outcome Definition

Every assessment must declare what `profitable` means and when it will be measured. The contract records a versioned prediction horizon and outcome definition, for example: positive net profit at broker-confirmed closure, target reached before stop-loss, or positive net profit within a specified time window. Pre-trade and live-trade predictions must not mix incompatible horizons, and calibration compares only like-for-like outcomes.

### 21.5 Light Stability and Anti-Flicker Control

Colour changes require configurable confirmation across consecutive calculations or a minimum dwell period. Small movements around a score boundary must use hysteresis so the dashboard does not rapidly alternate states. Hard-risk, Emergency Stop, CyberShield™, stale-data and execution-integrity downgrades bypass all delay and apply immediately. Every delayed or immediate transition is audited.

### 21.6 Broker-Confirmed Outcome Reconciliation

Every prediction must be reconciled automatically with the final broker-confirmed order and deal history, including partial closes, commission, swap, slippage and realized net profit. The reconciler links the assessment ID, trade ID, order/deal tickets and model version; detects missing or duplicated outcomes; and prevents provisional platform values from being used as verified training evidence.

### 21.7 Shadow Mode and Champion-Challenger Promotion

New weights, rules and machine-learning models run in Shadow Mode without influencing entries, exits or risk. Their decisions are recorded beside the approved champion model. Promotion requires out-of-sample, walk-forward, demo and controlled-live evidence showing improved calibration and acceptable drawdown, false-green rate and regime stability. CEO Approval Center authorization, signed versioning and rollback readiness remain mandatory.

### 21.8 Segmented Calibration

Reliability statistics must be measured separately by EA, magic number, symbol, timeframe, broker, direction, session, volatility band and market regime. Segments with inadequate sample size inherit only an approved conservative parent calibration and remain labelled low-confidence. Data from one EA or broker must not silently validate another.

### 21.9 Live Loss-of-Confidence Rule

A live Green assessment must be downgraded immediately when confidence collapses because of regime change, model disagreement, reversal evidence, liquidity loss, abnormal costs or degraded data quality—even if the raw score has not yet crossed a colour boundary. The system sends a protective recommendation to the Capital Execution Guardian and records the triggering evidence; it never closes a trade outside approved policy.

### 21.10 CEO Reliability Panel

The CEO Dashboard and AI Control Tower must report calibration error, probability interval coverage, false-green and false-red rates, net expected versus realized value, sample size, abstention rate, drift, override frequency and Profit Light performance by EA and regime. It must distinguish backtest, demo, shadow and controlled-live evidence. Alerts are generated when reliability limits are breached, and the affected model may be automatically restricted, rolled back or quarantined under approved governance.

### 21.11 Additional Acceptance Tests

Acceptance tests must prove that:

- a non-positive net expected value prevents Green regardless of the raw score;
- insufficient valid evidence produces Grey/Abstain rather than Black;
- uncertainty intervals and sample sizes are returned and stored;
- boundary noise does not cause repeated colour flicker;
- hard safety downgrades bypass stability delays;
- prediction horizons cannot be mixed during calibration;
- every verified outcome reconciles to broker deals and net costs;
- a challenger in Shadow Mode cannot influence trading actions;
- calibration remains isolated across EA, broker and regime segments; and
- a material live confidence collapse triggers an immediate audited downgrade.

## 22. Final Production Control Set

These controls are mandatory before EA Profit Light™ may influence any live protection recommendation. Profit Light remains decision support: it cannot create risk, bypass a safety authority, promise profit or certify itself for production.

### 22.1 Signed Configuration and Model Integrity

Every active score definition, colour band, weight, threshold, calibration table, cost model, prediction horizon and action mapping must belong to one signed, immutable release package. The EA adapter and platform must verify the same package ID and checksum before using a result. A missing signature, unauthorized change, incompatible version or checksum mismatch forces `Black/Unavailable`, blocks new entries and creates a critical incident.

### 22.2 Independent Data and Clock Validation

Market price, broker state, account state, economic-news state and portfolio exposure must carry source time, receive time and freshness limits. TRG compares server, terminal and broker clocks and rejects future-dated, out-of-order, duplicated or stale snapshots. When independent sources materially disagree, the light moves to Grey/Abstain or Black/Unavailable according to severity; it must never preserve an expired Green result.

### 22.3 Decision Lease and Revalidation

Every assessment is a short-lived decision lease bound to the exact EA, magic number, account, broker, symbol, timeframe, direction, proposed volume, stop-loss, take-profit and input hash. Any material change invalidates the lease and requires a complete recalculation. Entry Authority and the MT4/MT5 bridge must reject reused, altered or expired approvals.

### 22.4 Protective Action Acknowledgement

Every `Protect`, `Partial Close`, `Break-Even`, `Trail`, `Close` or `Emergency Close` recommendation receives a correlation ID, idempotency key, issue time, expiry and policy reference. Execution is not assumed: the bridge must return broker-confirmed order/deal tickets, filled volume, price, costs and remaining exposure. Missing, rejected or partial acknowledgements trigger bounded retry, reconciliation and escalation to the Capital Execution Guardian and Notifications & Alert Center.

### 22.5 Profit-Lock and Giveback Guard

For profitable positions, an approved policy may define a high-watermark, minimum protected amount and maximum permitted giveback after costs. As profit rises, protection may remain unchanged or tighten; it may not be loosened automatically to chase a larger target. The `$6,000` example remains a configurable monitoring/protection objective, not a guarantee and never permission to increase risk. If protection cannot be placed or verified, the Guardian selects the safest approved reduce-only response and alerts the CEO.

### 22.6 Restart, Failover and State Reconstruction

After a VPS, terminal, bridge, service or database restart, Profit Light must enter `Recovering/Unavailable`, block new entries and reconstruct state from broker-confirmed positions, orders, deals, active policies and the last immutable audit checkpoint. No cached Green assessment survives a restart. Normal calculation resumes only after reconciliation, health checks, version verification and fresh-data validation succeed.

### 22.7 Incident Latch and Controlled Recovery

A critical false-green event, unexplained state divergence, repeated execution failure, calibration breach or safety-control failure latches the affected EA, account, broker route or model in `Restricted` or `Quarantined`. Automatic recovery cannot clear the latch. Release requires root-cause evidence, corrected tests, replay and stress results, a defined observation period, rollback readiness and approval from the configured authority.

### 22.8 Explainability and No-Silent-Override Rule

Every displayed colour and recommended action must include the top supporting factors, opposing factors, active overrides, confidence, uncertainty range, data age, calculation version and expiry. A safety authority may downgrade or block the light but may never silently upgrade it. Dashboard, API, audit and EA adapter must show the same final state and machine-readable reason codes.

### 22.9 Production Readiness Certificate

Production activation requires one evidence package linking the exact source commit, MQL5 and .NET builds, compiler versions, test results, Strategy Tester reports, bridge-contract version, cybersecurity checks, broker/demo evidence, model/calibration report, configuration checksum, rollback package and approvals. Any code, model, weight, threshold, schema or safety-policy change invalidates the affected certificate and returns the component to the required validation stage.

### 22.10 Final Go-Live Acceptance

Go-live is blocked until TRG proves that:

- Profit Light cannot open a trade or increase exposure by itself;
- Emergency Stop, CyberShield™, hard-risk limits, the Capital Execution Guardian and Entry Authority always retain precedence;
- Grey and Black states fail closed and expired Green decisions cannot be reused;
- every assessment is identity-bound, time-limited, signed/versioned and reproducible;
- every protective action is idempotent, broker-confirmed and reconciled;
- restart and failover cannot restore stale permission or lose open-position protection;
- profit-lock rules prevent unauthorized giveback and never chase a target;
- a critical incident latches the affected scope until controlled release;
- dashboard, API, audit history and MT4/MT5 adapter return the same governed state; and
- complete rollback, alerting and evidence remain available during a simulated failure.

## 23. Permanent Architecture Decision

TRG-MARKETS adopts EA Profit Light™ as a permanent native decision-support and live-trade monitoring component of the Autonomous Trading OS™. Its official operating sequence is:

`Observe → Validate → Score → Apply Safety Authority → Explain → Recommend → Confirm Execution → Reconcile → Learn`

The system must prefer `Wait`, `Abstain`, `Unavailable` or a capital-protection action whenever evidence is insufficient, conflicting, stale or unsafe. **No colour, probability, model or profit objective can override verified capital protection.**
