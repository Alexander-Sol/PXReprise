---
title: "0070 Record engine decisions as MADR records in this repository"
parent: Decisions
nav_order: 70
status: accepted
date: 2026-11-14
decision-makers: the maintainer
---

# Record engine decisions as MADR records in this repository

## Context and Problem Statement

The engine's decisions were kept in the maintainers' private project records: a decision log, plans, and a long
instructions file for the AI agent that works on the project. Since this repository became the engine's real home
([0067](0067-smith-is-the-engine-repository.md)), contributors work here and cannot see those records. The instructions
file had also grown into a dated status log, in which old entries contradicted new ones.

## Considered Options

* Keep every decision in the private records
* Record engine decisions here, as MADR records reviewed by pull request; keep project and operations decisions private
* Move every decision here

## Decision Outcome

Chosen option: "Record engine decisions here", because the people who change the engine must be able to read why it
works as it does, and operations decisions are about one lab's machines.

* Engine decisions are recorded in `docs/decisions/`, in the MADR 4.0 minimal format, and added or changed by pull
  request.
* A record's number is its number in the maintainers' decision log.
* A decision that reverses a record gets a new record; the old one's status becomes `superseded by`.
* Project and batch-operations decisions stay in the maintainers' private project records.
* An AI agent's instructions file holds standing rules only, never a status log. Status and history go to the project's
  resume and journal.
* Records 0001-0069 were backfilled from the log on 2026-11-14.

### Consequences

* Good, because contributors see the decisions that bind the code, next to the code.
* Good, because a superseded decision is marked, not left to contradict its successor.
* Bad, because each engine decision is now written twice: a line in the private log and a record here.
