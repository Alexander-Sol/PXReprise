---
title: "0004 Follow mzLib's conventions for failures, async, the CLI and tests"
parent: Decisions
nav_order: 4
status: accepted
date: 2026-09-28
decision-makers: the maintainer
---

# Follow mzLib's conventions for failures, async, the CLI and tests

## Context and Problem Statement

The engine calls mzLib throughout and is reviewed by the same people. Which conventions should its own code follow?

## Considered Options

* Conventions of the engine's own
* mzLib's conventions, binding where mzLib's tests and CI enforce them

## Decision Outcome

Chosen option: "mzLib's conventions", because a reader who knows mzLib can read the engine, and code can move between
them.

* **Failures:** `ArgumentException` for a wrong call; `MzLibException` when a service answered but broke its contract,
  a checksum mismatch included; `HttpRequestException` only when a service is unavailable (408, 429, 5xx). An expected
  absence is a `Try*` method's result, not an exception.
* **Async:** a `CancellationToken` as the last parameter, `.ConfigureAwait(false)` throughout. Nullable reference types
  are enabled.
* **CLI:** one JSON envelope on stdout (`{"ok":true,"data":...}` or `{"ok":false,"error":{...}}`), diagnostics on
  stderr, exit codes 0 ok, 1 handled failure, 2 usage error, and a `version` verb that lists the verbs.
* **Tests:** NUnit. Offline tests use a stub `HttpMessageHandler`. Live tests carry `[Category("ExternalService")]`,
  skip on an outage, fail on anything else, and stay out of the required CI job.

### Consequences

* Good, because an agent or a script can rely on the CLI's envelope and exit codes.
* Good, because an outage never fails the required build.
* Bad, because a test that fakes a failure must throw the real exception type .NET throws (a dropped transfer is an
  `HttpIOException`, an `IOException`, not an `HttpRequestException`).
