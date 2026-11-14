---
title: "0001 C#, orchestration only: mzLib computes, MetaMorpheus searches"
parent: Decisions
nav_order: 1
status: accepted
date: 2026-09-27
decision-makers: the maintainer
---

# C#, orchestration only: mzLib computes, MetaMorpheus searches

## Context and Problem Statement

PXReprise began as a set of Python scripts that ran one biological question (aging). It had to become a generic
engine that any question can use. The lab's own tools, mzLib and MetaMorpheus, are C#. Which language should the engine
use, and how much of the science should live inside it?

## Considered Options

* Keep the Python scripts and generalise them
* C#, with the chemistry, readers and statistics written inside the engine
* C#, with the engine doing orchestration, policy and provenance only

## Decision Outcome

Chosen option: "C#, orchestration only", because every algorithm then has one implementation, shared with the lab's
other tools.

* The engine is C# on .NET 10, written in mzLib's and MetaMorpheus's style.
* Nothing algorithmic lives in the engine. Chemistry, spectra readers, SDRF, quantification and statistics are mzLib's.
  Search is MetaMorpheus's.
* MetaMorpheus runs as its own process, `dotnet CMD.dll`, one process per dataset. Its task layer is not packaged as a
  library, its events are static and its global settings belong to the process, so it cannot safely run inside the
  engine. (This record also covers entry 3 of the log.)

### Consequences

* Good, because a fix in mzLib or MetaMorpheus reaches PXReprise, and the reverse.
* Good, because the engine may use a newer mzLib than MetaMorpheus does: two mzLib versions never share a process.
* Bad, because a missing capability means a change upstream first, and PXReprise waits for that release.
* Before writing a new class, check whether mzLib or MetaMorpheus already has it.
