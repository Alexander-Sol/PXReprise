---
title: "0009 Time and size limits belong to the machine, not the profile"
parent: Decisions
nav_order: 9
status: accepted
date: 2026-09-29
decision-makers: the maintainer
---

# Time and size limits belong to the machine, not the profile

## Context and Problem Statement

How long a search takes depends on the computer and its load, not on the method. One deposit (PXD069093) took 5 h 58 min
on a busy computer against a 6-hour limit in its profile, and was two minutes from being killed while it was working.
Where should limits live?

## Considered Options

* Limits in each profile
* Limits in the machine file

## Decision Outcome

Chosen option: "Limits in the machine file", because a slow or busy computer must not change the method.

* `search_timeout_h` (default 48) is a ceiling on a search.
* `search_stall_minutes` (default 90) kills only a search whose MetaMorpheus used no CPU for that long: hung, not slow.
* A profile's own timeout is read and not used.
* **Deposit size gates** (entry 42 of the log, 2026-10-17) moved the same way. The machine file's `[deposit_limits]`
  (`max_files`, `max_file_mb`, `max_est_ms2`, `defer_if_median_file_mb`) replaced the profiles' gates. A running driver
  reads them again when the file changes, and a deposit deferred for size gets another look when the new limits fit what
  its deferral measured. Size gates choose which deposits are searched, never how, so changing them is not a new
  profile version.

### Consequences

* Good, because a lab can raise limits step by step as its hardware allows, without touching a profile.
* Bad, because a hung search can hold its slot for up to `search_stall_minutes` before it is killed.
